using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Services.RoleServices;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebApp.Areas.Administrator.Controllers
{
    [ApiController]
    [Route("api/Role")]
    [Authorize]
    public class RolesController : BaseController<IRoleService>
    {
        public RolesController(IRoleService service) : base(service)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<RoleFilter> query)
        {
            query.PageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            query.PageSize = query.PageSize > 0 ? query.PageSize : 10;
            var roles = await _service.GetPageList(query);
            var response = new
            {
                TotalRecords = roles.TotalRecords,
                Items = roles.Items
            };
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(RoleQuery query)
        {
            var result = await _service.CreateAsync(query);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(RoleQuery query)
        {
            var result = await _service.UpdateAsync(query, query.Name);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

    }
}
