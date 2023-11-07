
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebAdmin.Controllers.Base
{
    public class BaseController<T> : Controller
    {
        protected readonly T _service;
        public BaseController(T service)
        {
            _service = service;
        }

        protected IActionResult ResponseSuccess(object data, string? message = null)
        {
            return Ok(new
            {
                Success = true,
                Items = data,
                Message = message ?? "Success",
            });
        }
        // response error
        protected IActionResult ResponseError(string message)
        {
            return Ok(new
            {
                Success = false,
                Message = new string[] { message },
            });
        }
        // response error
        protected IActionResult ResponseErrors(List<string> messages)
        {
            return Ok(new
            {
                Success = false,
                Message = messages,
            });
        }

    }
}
