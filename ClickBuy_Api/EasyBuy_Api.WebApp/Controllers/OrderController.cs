using ClickBuy_Api.Common;
using ClickBuy_Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickBuy_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        string currentPath = Directory.GetCurrentDirectory() + "\\Data\\";
        public OrderController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            string path = currentPath + "order.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false });
            }
           List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(path));
            return Ok(
                new { success = true, data = orders }
            );

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string path = currentPath + "order.json";
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(json);
                var order = orders.FirstOrDefault(x => x.Id == id);
                if (order == null)
                {
                    return Ok(new { success = false });
                }
                return Ok(
                    new { success = true, data = order }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {

            string cardPath = currentPath + "card.json";
            if (order.NamePayment == "Chuyển khoảng")
            {
                var cardsJson = await System.IO.File.ReadAllTextAsync(cardPath);
                var cards = JsonConvert.DeserializeObject<List<Card>>(cardsJson);

                var card = cards.FirstOrDefault(c => c.Name == order.BankName && c.BankNumber == order.BankNumber);
                if (card == null)
                {
                    return BadRequest(new { success = false, message = "Invalid card information" });
                }

                decimal amountToSubtract = order.SubTotal;
                if (card.Balance < amountToSubtract)
                {
                    return BadRequest(new { success = false, message = "Insufficient balance" });
                }

                card.Balance -= amountToSubtract;

                string updatedCardsJson = JsonConvert.SerializeObject(cards);
                await System.IO.File.WriteAllTextAsync(cardPath, updatedCardsJson);
            }
            string orderPath = currentPath + "order.json";
            List<Order> orders = new List<Order>();

            if (!System.IO.File.Exists(orderPath))
            {
                using (StreamWriter sw = System.IO.File.CreateText(orderPath))
                {
                    sw.WriteLine("[]");
                }
            }

            var productsJson = await System.IO.File.ReadAllTextAsync(orderPath);
            orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(productsJson);
            order.Id = (orders != null && orders.Count > 0) ? ClickBuyHelper.AutoIncrement(orders.OrderByDescending(x => x.Id).FirstOrDefault().Id) : 1;
            order.Name = order.Name;
            order.Address = order.Address;
            order.NamePayment = order.NamePayment;
            order.BankNumber = order.BankNumber;
            order.BankName = order.BankName;
            order.CreatedAt = DateTime.Now;
            orders.Add(order);

            string newJson = JsonConvert.SerializeObject(orders);
            await System.IO.File.WriteAllTextAsync(orderPath, newJson);

            return Ok(new { success = true, message = "Create success" });
        }

        [HttpPut]
        public IActionResult Update(Order order)
        {
            string path = currentPath + "order.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false, message = "File not found" });
            }
            List<Order> orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(System.IO.File.ReadAllText(path));
            var orderExist = orders.FirstOrDefault(x => x.Id == order.Id);
            if (orderExist == null)
            {
                return Ok(new { success = false, message = "Order not exist" });
            }
            orderExist = order;
            string jsonWrite = JsonConvert.SerializeObject(orders);
            System.IO.File.WriteAllText(path, jsonWrite);
            return Ok(new { success = true, message = "Update order success" });
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string path = currentPath + "order.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false, message = "File not found" });
            }
            List<Order> orders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(System.IO.File.ReadAllText(path));
            var orderExist = orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return Ok(new { success = false, message = "Order not exist" });
            }
            orders.Remove(orderExist);
            string jsonWrite = JsonConvert.SerializeObject(orders);
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                sw.WriteLine(jsonWrite);
            }
            return Ok(new { success = true, message = "Delete order success" });
        }
    }
}