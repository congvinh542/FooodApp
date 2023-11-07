using ClickBuy_Api.Service.Services.DashboardServices;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebApp.Areas.Administrator.Controllers
{
    [ApiController]
    [Route("api/Permission")]
    [Authorize]
    public class PermissionController : BaseController<IDashboardService>
    {
        public PermissionController(IDashboardService service) : base(service)
        {
        }

    }
}
