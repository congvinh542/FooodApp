using ClickBuy_Api.Common;
using ClickBuy_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickBuy_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        string currentPath = Directory.GetCurrentDirectory() + "\\Data\\";
        public CartController()
        {

        }



        [HttpGet]
        public IActionResult Get()
        {
            string path = currentPath + "cart.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false });
            }
            List<Cart> carts = System.Text.Json.JsonSerializer.Deserialize<List<Cart>>(System.IO.File.ReadAllText(path));
            return Ok(
                new { success = true, data = carts }
            );
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string path = currentPath + "cart.json";
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(json);
                var cart = carts.FirstOrDefault(x => x.Id == id);
                if (cart == null)
                {
                    return Ok(new { success = false });
                }
                return Ok(
                    new { success = true, data = cart }
                );
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cart cart)
        {
            string path = currentPath + "cart.json";
            List<Cart> carts = new List<Cart>();
            if (!System.IO.File.Exists(path))
            {
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine("[]");
                }
            }
            var cartJson = await System.IO.File.ReadAllTextAsync(path);
            carts = System.Text.Json.JsonSerializer.Deserialize<List<Cart>>(cartJson);

            cart.Id = carts.Count > 0 ? ClickBuyHelper.AutoIncrement(carts.OrderByDescending(x => x.Id).FirstOrDefault().Id) : 1;
            cart.CreatedAt = DateTime.Now;
            cart.TotalPrice = cart.Product.Price * cart.Quantity;
            carts.Add(cart);
            string newJson = JsonConvert.SerializeObject(carts);
            await System.IO.File.WriteAllTextAsync(path, newJson);
            return Ok(
                new { success = true, message = "Create success" }  
            );

        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Cart cart)
        {
            string path = currentPath + "cart.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false, message = "File not found" });
            }
            List<Cart> carts = System.Text.Json.JsonSerializer.Deserialize<List<Cart>>(System.IO.File.ReadAllText(path));
            var cartExist = carts.FirstOrDefault(x => x.Id == id);
            if (cartExist == null)
            {
                return Ok(new { success = false, message = "Cart not found" });
            }
            cartExist.Quantity = cart.Quantity;
            cartExist.TotalPrice = cart.TotalPrice;
            string jsonWrite = JsonConvert.SerializeObject(carts);
            System.IO.File.WriteAllText(path, jsonWrite);
            return Ok(new { success = true, data = cartExist });

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string path = currentPath + "cart.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false, message = "File not found" });
            }
            List<Cart> carts = System.Text.Json.JsonSerializer.Deserialize<List<Cart>>(System.IO.File.ReadAllText(path));
            var cartExist = carts.FirstOrDefault(x => x.Id == id);
            if (cartExist == null)
            {
                return Ok(new { success = false, message = "Cart not found" });
            }
            carts.Remove(cartExist);
            string jsonWrite = JsonConvert.SerializeObject(carts);
            System.IO.File.WriteAllText(path, jsonWrite);
            return Ok(new { success = true, data = cartExist });
        }
    }
}