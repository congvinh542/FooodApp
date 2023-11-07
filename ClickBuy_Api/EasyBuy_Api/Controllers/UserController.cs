using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Services.Genders;
using ClickBuy_Api.Service.Services.Huyen;
using ClickBuy_Api.Service.Services.Images;
using ClickBuy_Api.Service.Services.Tinhs;
using ClickBuy_Api.Service.Services.UserServices;
using ClickBuy_Api.Service.Services.Xas;
using ClickBuy_Api.WebAdmin.Controllers.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClickBuy_Api.WebAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController<IUserService>
    {
        private readonly IimageService _imageService;
        private readonly IGenderService _genderService;
        private readonly ITinhService _tinhService;
        private readonly IHuyenService _huyenService;
        private readonly IXaService _xaService;

        public UsersController(IUserService service, IimageService imageService,
                               IGenderService genderService, ITinhService tinhService, IHuyenService huyenService,
                               IXaService xaService) : base(service)
        {
            _imageService = imageService;
            _genderService = genderService;
            _tinhService = tinhService;
            _huyenService = huyenService;
            _xaService = xaService;
        }

        [HttpGet("create")]
        public async Task<IActionResult> GetCreateView()
        {
            var image = await _imageService.GetAll();
            var gender = await _genderService.GetAll();
            var tinh = await _tinhService.GetAll();
            var huyen = await _huyenService.GetAll();
            var xa = await _xaService.GetAll();

            ViewBag.Images = image.Items;
            ViewBag.Genders = gender.Items;
            ViewBag.Tinhs = tinh.Items;
            ViewBag.Huyens = huyen.Items;
            ViewBag.Xas = xa.Items;

            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<UserFilter> query)
        {
            query.PageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            query.PageSize = query.PageSize > 0 ? query.PageSize : 10;
            var users = await _service.GetPageList(query);
            var response = new
            {
                TotalRecords = users.TotalRecords,
                Items = users.Items
            };
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserQuery query)
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
        public async Task<IActionResult> Update([FromBody] UserQuery query, string id)
        {
            var result = await _service.UpdateAsync(query, id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth", new { area = "" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            var result = await _service.Login(query);
            if (result.Success == false)
            {
                return Ok(result);
            }
            //Login MVC
            var user = result.Entity as UserView;
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username),
                };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(new[] { identity });
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties { IsPersistent = true });
            return Ok(result);
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> GetUpdateView(string id)
        {
            var image = await _imageService.GetAll();
            var gender = await _genderService.GetAll();
            var tinh = await _tinhService.GetAll();
            var huyen = await _huyenService.GetAll();
            var xa = await _xaService.GetAll();

            ViewBag.Images = image.Items;
            ViewBag.Genders = gender.Items;
            ViewBag.Tinhs = tinh.Items;
            ViewBag.Huyens = huyen.Items;
            ViewBag.Xas = xa.Items;

            var user = await _service.GetByIdAsync(id);
            return View(user);
        }
    }
}