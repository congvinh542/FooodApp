using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Services.Genders;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebAdmin.Controllers
{
    [ApiController]
    [Route("api/Gender")]
    [Authorize]
    public class GenderController : BaseController<IGenderService>
    {
        public GenderController(IGenderService service) : base(service)
        {
        }

        #region CRUD
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<GenderFilter> query)
        {
            query.PageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            query.PageSize = query.PageSize > 0 ? query.PageSize : 10;
            var genders = await _service.GetPageList(query);
            var response = new
            {
                TotalRecords = genders.TotalRecords,
                Items = genders.Items
            };
            return Ok(response);
        }

        [HttpGet("{GetId}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(GenderQuery query)
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
        public async Task<IActionResult> Update(GenderQuery query)
        {
            var result = await _service.UpdateAsync(query, query.Name);
            return Ok(result);
        }
        #endregion
    }
}
