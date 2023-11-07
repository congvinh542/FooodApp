using ClickBuy_Api.Common;
using ClickBuy_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickBuy_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        string currentPath = Directory.GetCurrentDirectory() + "\\Data\\";
        public CategoryController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            string path = currentPath + "category.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(
                    new { success = false, message = "Not found category" }
                );
            }
            List<Category> categorys = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(path));
            var items = categorys.OrderByDescending(x => x.Id).ToList();
            return Ok(
                new { success = true, data = items }
            );
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string path = currentPath + "category.json";
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<Category> Categorys = JsonConvert.DeserializeObject<List<Category>>(json);
                var category = Categorys.FirstOrDefault(x => x.Id == id);
                if (category == null)
                {
                    return Ok(new { success = false });
                }
                return Ok(
                    new { success = true, data = category }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            string path = currentPath + "category.json";
            List<Category> categorys = new List<Category>();
            if (!System.IO.File.Exists(path))
            {
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine("[]");
                }
            }
            var produtsJson = await System.IO.File.ReadAllTextAsync(path);
            categorys = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(produtsJson);
            category.Id = (categorys != null && categorys.Count > 0) ? ClickBuyHelper.AutoIncrement(categorys.OrderByDescending(x => x.Id).FirstOrDefault().Id) : 1;
            categorys.Add(category);

            string newJson = JsonConvert.SerializeObject(categorys);
            await System.IO.File.WriteAllTextAsync(path, newJson);
            return Ok(
                new { success = true, message = "Create success" }
            );

        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            string path = currentPath + "category.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false, message = "File not found" });
            }
            List<Category> Categorys = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(path));
            var categoryUpdate = Categorys.FirstOrDefault(x => x.Id == category.Id);
            if (categoryUpdate == null)
            {
                return Ok(new { success = false, message = "Category not exist" });
            }
            categoryUpdate.Name = category.Name;
            string jsonWrite = JsonConvert.SerializeObject(Categorys);
            System.IO.File.WriteAllText(path, jsonWrite);
            return Ok(new { success = true, message = "Update Category success" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string path = currentPath + "Category.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { success = false, message = "File not found" });
            }
            List<Category> categorys = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(path));
            var categoryExist = categorys.FirstOrDefault(x => x.Id == id);
            if (categoryExist == null)
            {
                return Ok(new { success = false, message = "Category not exist" });
            }
            categorys.Remove(categoryExist);
            string jsonWrite = JsonConvert.SerializeObject(categorys);
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                sw.WriteLine(jsonWrite);
            }
            return Ok(new { success = true, message = "Delete Category success" });
        }
    }
}
