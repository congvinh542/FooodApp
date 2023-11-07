using ClickBuy_Api.Database.Entities;
using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.SDK.Extensions;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using ClickBuy_Api.Service.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Services.PermissionServices
{
    public class PermissionService : ServiceBase<IUnitOfWork>, IPermissionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public PermissionService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<Role> roleManager) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<DataResult<bool>> AddToRolePermissionAsync(Guid roleId, Guid permissionId)
        {
            var result = new DataResult<bool>();
            var rolePermission = await _unitOfWork.GetRepository<RolePermission>()
            .GetByCondition(x => x.RoleId == roleId && x.PermissionId == permissionId)
            .FirstOrDefaultAsync();
            if (rolePermission != null)
            {
                result.Errors.Add("Permission already exists");
                return result;
            }
            var newRolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
            };
            await _unitOfWork.GetRepository<RolePermission>().Add(newRolePermission);
            result.Entity = await _unitOfWork.SaveChangesAsync() > 0;
            if (result.Entity == false)
            {
                result.Errors.Add("Error while saving");
                return result;
            }
            return result;
        }

        public async Task<DataResult<bool>> CreateAsync(PermissionQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<PermissionQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }

            var permission = new Permission
            {
                Value = entity.PermissionValue,
                Key = entity.PermissionKey,
                Description = entity.Description,
                CreatedBy = entity.CreatedBy,
            };
            await _unitOfWork.GetRepository<Permission>().Add(permission);
            result.Entity = await _unitOfWork.SaveChangesAsync() > 0;
            if (result.Entity == false)
            {
                result.Errors.Add("Error while saving");
                return result;
            }
            return result;
        }

        public async Task<DataResult<int>> DeleteAsync(string id)
        {
            var result = new DataResult<int>();
            var permission = await _unitOfWork.GetRepository<Permission>().GetByIdAsync(Guid.Parse(id));
            if (permission == null)
            {
                result.Errors.Add("Permission not found");
                return result;
            }
            _unitOfWork.GetRepository<Permission>().Delete(permission, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<PermissionView>> GetAllRolePermissionsAsync(BaseFilter<PermissionFilter> query)
        {
            var response = new DataResult<PermissionView>();
            var permission = await _unitOfWork.GetRepository<RolePermission>().AsQueryable()
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value)
                .Include(x => x.Permission)
                .Include(x => x.Role)
                .Select(x => new PermissionView()
                {
                    Id = x.Id,
                    PermissionKey = x.Permission.Key,
                    PermissionId = x.Permission.Id,
                    RoleId = x.Role.Id,
                    PermissionValue = x.Permission.Value,
                    Description = x.Permission.Description,
                    CreatedBy = x.Permission.CreatedBy,
                    CreatedAt = x.Permission.CreatedAt,
                    UpdatedAt = x.Permission.UpdatedAt,
                    IsActive = x.Permission.IsActive,
                    RoleName = x.Role.Name,

                })
                .ApplyFilter(query)
                .OrderByColums(query.SortColums, true)
                .ToListAsync();
            response.Items = permission;
            response.TotalRecords = await _unitOfWork.GetRepository<Permission>().AsQueryable().CountAsync();
            return response;
        }

        public async Task<DataResult<PermissionView>> GetByIdAsync(string id)
        {
            var result = new DataResult<PermissionView>();
            var permission = await _unitOfWork.GetRepository<Permission>().GetByIdAsync(Guid.Parse(id));
            if (permission == null)
            {
                result.Errors.Add("Permission not found");
                return result;
            }
            result.Entity = new PermissionView
            {
                Id = permission.Id,
                PermissionKey = permission.Key,
                PermissionValue = permission.Value,
                Description = permission.Description,
                CreatedBy = permission.CreatedBy,
                CreatedAt = permission.CreatedAt,
                UpdatedAt = permission.UpdatedAt,
                IsActive = permission.IsActive,
            };
            return result;
        }

        public async Task<DataResult<RoleView>> GetByRoleAsync(Guid roleId)
        {
            var response = new DataResult<RoleView>();
            var permission = await _unitOfWork.GetRepository<Permission>().AsQueryable()
                .Include(x => x.RolePermissions)
                .Where(x => x.RolePermissions.Any(y => y.RoleId == roleId))
                .Select(x => new RoleView()
                {
                    RoleName = x.RolePermissions.FirstOrDefault().Role.Name,
                    Permissions = x.RolePermissions.Select(y => new PermissionView()
                    {
                        Id = y.Permission.Id,
                        PermissionKey = y.Permission.Key,
                        PermissionValue = y.Permission.Value,
                        Description = y.Permission.Description,
                        CreatedBy = y.Permission.CreatedBy,
                        CreatedAt = y.Permission.CreatedAt,
                        UpdatedAt = y.Permission.UpdatedAt,
                        IsActive = y.Permission.IsActive,
                    }).ToList(),
                }).ToListAsync();
            response.Items = permission;
            return response;
        }

        public async Task<DataResult<RoleView>> GetByRoleAsync(string roleName)
        {
            var response = new DataResult<RoleView>();
            var permission = await _unitOfWork.GetRepository<RolePermission>().AsQueryable()
            .Include(x => x.Permission)
            .Include(x => x.Role)
            .Select(x => new RoleView()
            {
                RoleName = x.Role.Name,
                Permissions = x.Role.RolePermissions.Where(y => y.Role.Name == roleName).Select(y => new PermissionView()
                {
                    Id = y.Permission.Id,
                    PermissionKey = y.Permission.Key,
                    PermissionValue = y.Permission.Value,
                    Description = y.Permission.Description,
                    CreatedBy = y.Permission.CreatedBy,
                    CreatedAt = y.Permission.CreatedAt,
                    UpdatedAt = y.Permission.UpdatedAt,
                    IsActive = y.Permission.IsActive,
                }).ToList(),
            }).FirstOrDefaultAsync(x => x.RoleName == roleName);
            response.Entity = permission;
            return response;

        }

        public async Task<DataResult<UserView>> GetByUserAsync(Guid userId)
        {
            var response = new DataResult<UserView>();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                response.Errors.Add("User not found");
                return response;
            }
            // Get all roles of the user and their permissions
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await _unitOfWork.GetRepository<RolePermission>()
                .AsQueryable()
                .Include(rp => rp.Permission)
                .Include(rp => rp.Role)
                .Where(rp => roles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission)
                .ToListAsync();
            // Merge all permissions into a single array
            var allPermissions = permissions.Select(p => new PermissionView()
            {
                Id = p.Id,
                PermissionKey = p.Key,
                PermissionValue = p.Value,
                Description = p.Description,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsActive = p.IsActive,
            }).ToList();
            // build the user view model
            response.Entity = new UserView()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                RolesView = new List<RoleView>()
                {
                    new RoleView()
                    {
                        RoleName = roles.FirstOrDefault(),
                        Permissions = allPermissions
                    }
                }
            };

            return response;
        }

        public async Task<DataResult<UserView>> GetByUserAsync(string username)
        {

            var response = new DataResult<UserView>();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                response.Errors.Add("User not found");
                return response;
            }

            // Get all roles of the user and their permissions
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await _unitOfWork.GetRepository<RolePermission>()
                .AsQueryable()
                .Include(rp => rp.Permission)
                .Include(rp => rp.Role)
                .Where(rp => roles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission)
                .ToListAsync();

            // Merge all permissions into a single array
            var allPermissions = permissions.Select(p => new PermissionView()
            {
                Id = p.Id,
                PermissionKey = p.Key,
                PermissionValue = p.Value,
                Description = p.Description,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsActive = p.IsActive
            }).ToList();

            // Build the UserView object
            var userView = new UserView()
            {
                Id = user.Id,
                Username = user.UserName,
                RolesView = new List<RoleView>()
                {
                    new RoleView()
                    {
                        RoleName = string.Join(", ", roles),
                        Permissions = allPermissions
                    }
                }
            };

            response.Entity = userView;
            return response;
        }

        public async Task<DataResult<PermissionView>> GetPageList(BaseFilter<PermissionFilter> query)
        {
            var permission = await _unitOfWork.GetRepository<Permission>().AsQueryable()
                    .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                    .Take(query.PageSize.Value)
                    .Select(x => new PermissionView()
                    {
                        Id = x.Id,
                        PermissionKey = x.Key,
                        PermissionValue = x.Value,
                        Description = x.Description,

                        CreatedBy = x.CreatedBy,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        IsActive = x.IsActive,
                    })
                    .ApplyFilter(query)
                    .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<PermissionView>();
            response.TotalRecords = await _unitOfWork.GetRepository<Permission>().AsQueryable().CountAsync();
            response.Items = permission;
            return response;
        }

        public async Task<DataResult<int>> RemoveRolePermissionAsync(Guid roleId, Guid permissionId)
        {
            var result = new DataResult<int>();
            var rolePermission = await _unitOfWork.GetRepository<RolePermission>().AsQueryable()
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId);

            _unitOfWork.GetRepository<RolePermission>().Delete(rolePermission, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<int>> UpdateAsync(PermissionQuery entity, string id)
        {
            var result = new DataResult<int>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<PermissionQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }

            var permission = await _unitOfWork.GetRepository<Permission>().GetByIdAsync(Guid.Parse(id));
            if (permission == null)
            {
                result.Errors.Add("Permission not found");
                return result;
            }
            permission.Value = entity.PermissionValue;
            permission.Key = entity.PermissionKey;
            permission.Description = entity.Description;
            permission.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Permission>().Update(permission);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<bool>> UpdateRolePermissionAsync(PermissionQuery entity, Guid roleId, Guid permissionId)
        {
            var result = new DataResult<bool>();
            var rolePermission = await _unitOfWork.GetRepository<RolePermission>().AsQueryable()
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId);
            if (rolePermission == null)
            {
                result.Errors.Add("Role permission not found");
                return result;
            }
            rolePermission.PermissionId = entity.PermissionId.Value;
            rolePermission.RoleId = entity.RoleId.Value;

            rolePermission.IsActive = entity.IsActive;
            rolePermission.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<RolePermission>().Update(rolePermission);
            result.Entity = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }
    }
}
