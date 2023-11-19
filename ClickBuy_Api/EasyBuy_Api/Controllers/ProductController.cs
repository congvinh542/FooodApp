using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Services.Images;
using ClickBuy_Api.Service.Services.Products;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebAdmin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : BaseController<IProductService>
    {
        private readonly IimageService _imageService;

        public ProductController(IProductService service, IimageService imageService) : base(service)
        {
            _imageService = imageService;
        }

        [HttpGet("create")]
        public async Task<IActionResult> GetCreateView()
        {
            var image = await _imageService.GetAll();

            ViewBag.Images = image.Items;

            return View();
        }

        #region CRUD
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<ProductFilter> query)
        {
            query.PageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            query.PageSize = query.PageSize > 0 ? query.PageSize : 10;
            var products = await _service.GetPageList(query);
            return Ok(products);
        }

        [HttpGet("ProductByCategory")]
        public async Task<IActionResult> GetByProductByCategory(string categoryId)
        {
            var result = await _service.GetProductsByCategory(categoryId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductQuery query)
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
        public async Task<IActionResult> Update([FromBody] ProductQuery query)
        {
            var result = await _service.UpdateAsync(query, query.Name);
            return Ok(result);
        }

        [HttpGet("drinks")]
        public async Task<IActionResult> GetDrinks()
        {
            var result = await _service.GetDrinksAsync();
            return Ok(result);
        }

        [HttpGet("bestsellers")]
        public async Task<IActionResult> GetBestSellers()
        {
            var result = await _service.GetBestSellersAsync();
            return Ok(result);
        }

        #endregion
    }
}
