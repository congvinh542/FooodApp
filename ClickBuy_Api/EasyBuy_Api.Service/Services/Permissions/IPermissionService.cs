using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.PermissionServices
{
    public interface IPermissionService : IBaseService<PermissionQuery, PermissionView, PermissionFilter>
    {
        Task<DataResult<bool>> AddToRolePermissionAsync(Guid roleId, Guid permissionId);
        Task<DataResult<int>> RemoveRolePermissionAsync(Guid roleId, Guid permissionId);
        Task<DataResult<bool>> UpdateRolePermissionAsync(PermissionQuery entity, Guid roleId, Guid permissionId);

        // Get by role id
        Task<DataResult<RoleView>> GetByRoleAsync(Guid roleId);
        Task<DataResult<RoleView>> GetByRoleAsync(string roleName);
        // get by user 
        Task<DataResult<UserView>> GetByUserAsync(Guid userId);
        Task<DataResult<UserView>> GetByUserAsync(string username);

        // get all  role permissions
        Task<DataResult<PermissionView>> GetAllRolePermissionsAsync(BaseFilter<PermissionFilter> query);
    }
}
