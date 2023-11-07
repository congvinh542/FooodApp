using System.ComponentModel.DataAnnotations;
using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using ClickBuy_Api.Service.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClickBuy_Api.Service.Services.RoleServices
{
    public class RoleService : ServiceBase<IUnitOfWork>, IRoleService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RoleManager<Role> _roleManager;
       public RoleService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, RoleManager<Role> roleManager) : base(unitOfWork)
		{
			_serviceProvider = serviceProvider;
			_roleManager = roleManager;
		}
        public async Task<DataResult<bool>> CreateAsync(RoleQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<RoleQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
			// Create
			var role = new Role
			{
				Name = entity.Name,
				NormalizedName = entity.Name.ToUpper(),
				ConcurrencyStamp = Guid.NewGuid().ToString()
			};
			var createResult = await _roleManager.CreateAsync(role);
			if (!createResult.Succeeded)
			{
				result.Errors.AddRange((IEnumerable<string>)createResult.Errors.Select(x => new ValidationResult(x.Description)));
				return result;
			}
			result.Entity = true;
			return result;

        }

        public async Task<DataResult<int>> DeleteAsync(string id)
        {
            var result = new DataResult<int>();
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				result.Errors.Add("Role is not exist");
				return result;
			}
			var deleteResult = await _roleManager.DeleteAsync(role);
			if (!deleteResult.Succeeded)
			{
				result.Errors.AddRange((IEnumerable<string>)deleteResult.Errors.Select(x => new ValidationResult(x.Description)));
				return result;
			}
			result.Entity = 1;
			return result;
        }

        public async Task<DataResult<RoleView>> GetByIdAsync(string id)
        {
            var result = new DataResult<RoleView>();
			var role = await _roleManager.FindByNameAsync(id);
			if (role == null)
			{
				result.Errors.Add("Role is not exist");
				return result;
			}
			result.Entity = new RoleView
			{
				Id = role.Id,
				RoleName = role.Name,
				
			};
			return result;

        }

        public async Task<DataResult<RoleView>> GetPageList(BaseFilter<RoleFilter> query)
        {
            var roles = await _roleManager.Roles.Skip((query.PageNumber.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value)
			.Select(x => new RoleView
			{
				Id = x.Id,
				RoleName = x.Name,
				
			}).ToListAsync();
			
			return new DataResult<RoleView>
			{
				Items = roles,
				TotalRecords = await _roleManager.Roles.CountAsync()
			};
        }

        public async Task<DataResult<int>> UpdateAsync(RoleQuery entity, string id)
        {
            var result = new DataResult<int>();
			var role = await _roleManager.FindByNameAsync(id);
			if (role == null)
			{
				result.Errors.Add("Role is not exist");
				return result;
			}
			role.Name = entity.Name;
			role.NormalizedName = entity.Name.ToUpper();
			role.ConcurrencyStamp = Guid.NewGuid().ToString();
			var updateResult = await _roleManager.UpdateAsync(role);
			if (!updateResult.Succeeded)
			{
				result.Errors.AddRange((IEnumerable<string>)updateResult.Errors.Select(x => new ValidationResult(x.Description)));
				return result;
			}
			result.Entity = 1;
			return result;

        }
    }
}
