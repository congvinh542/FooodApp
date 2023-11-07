using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Services.OrderDetails;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebAdmin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class OrderDetailsController : BaseController<IOrderDetailsService>
    {
        public OrderDetailsController(IOrderDetailsService service) : base(service)
        {
        }

        #region CRUD
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<OrderDetailsFilter> query)
        {
            query.PageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            query.PageSize = query.PageSize > 0 ? query.PageSize : 10;
            var tinhs = await _service.GetPageList(query);
            var response = new
            {
                TotalRecords = tinhs.TotalRecords,
                Items = tinhs.Items
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(OrderDetailQuery query)
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
        public async Task<IActionResult> Update(OrderDetailQuery query)
        {
            var result = await _service.UpdateAsync(query, query.Name);
            return Ok(result);
        }
        #endregion
    }
}
