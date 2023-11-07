using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Services.Huyen;
using ClickBuy_Api.Service.Services.Tinhs;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebAdmin.Controllers
{
    [ApiController]
    [Route("api/Huyen")]
    [Authorize]
    public class HuyenController : BaseController<IHuyenService>
    {
        private readonly ITinhService _tinhService;
        public HuyenController(IHuyenService service, ITinhService tinhService) : base(service)
        {
            _tinhService = tinhService;
        }


        [HttpGet("create")]
        public async Task<IActionResult> GetCreateView()
        {
            var tinh = await _tinhService.GetAll();

            ViewBag.Tinhs = tinh.Items;

            return View();
        }

        #region CRUD
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<HuyenFilter> query)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(HuyenQuery query)
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
        public async Task<IActionResult> Update(HuyenQuery query)
        {
            var result = await _service.UpdateAsync(query, query.Name);
            return Ok(result);
        }
        #endregion
    }
}
