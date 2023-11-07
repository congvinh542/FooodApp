using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.RoleServices
{
	public interface IRoleService : IBaseService<RoleQuery, RoleView, RoleFilter>
	{
		
	}
}
