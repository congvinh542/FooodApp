using ClickBuy_Api.Service.Services.PermissionServices;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebApp.Areas.Administrator.Controllers
{
    [ApiController]
    [Route("api/Permissions")]
    [Authorize]
    public class PermissionsController : BaseController<IPermissionService>
    {
        public PermissionsController(IPermissionService service) : base(service)
        {
        }

    }
}
