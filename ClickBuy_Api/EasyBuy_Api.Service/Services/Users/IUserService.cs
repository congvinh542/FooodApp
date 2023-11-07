using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
namespace ClickBuy_Api.Service.Services.UserServices
{
    public interface IUserService : IBaseService<UserQuery, UserView, UserFilter>
    {
        Task<DataResult<UserView>> Register(RegisterQuery entity);
        Task<DataResult<UserView>> Login(LoginQuery entity);
        Task<DataResult<UserView>> GetByUserName(string userName);
        Task<DataResult<bool>> ChangeRole(ChangeRoleQuery entity);
        Task<DataResult<bool>> UpdatePassword(UpdatePasswordQuery entity);
        Task<DataResult<bool>> UploadAvatar(UploadAvatarQuery entity);
        Task<DataResult<bool>> ForgotPassword(string email, string host);
        Task<DataResult<bool>> ResetPassword(UpdatePasswordQuery entity);
        Task<DataResult<bool>> LockUser(string username, bool isLock);
        Task<DataResult<UserView>> GetAll();
    }
}
