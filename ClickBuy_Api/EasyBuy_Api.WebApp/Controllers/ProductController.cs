using ClickBuy_Api.Common;
using ClickBuy_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickBuy_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // folder Data in project
        string currentPath = Directory.GetCurrentDirectory() + "\\Data\\";
        public ProductController()
        {

        }

        [HttpGet("search")]
        public IActionResult Search(string? productName)
        {
            string productsPath = Path.Combine(currentPath, "products.json");
            if (!System.IO.File.Exists(productsPath))
            {
                return Ok(new { success = false, message = "Products not found" });
            }

            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath));

            string categoriesPath = Path.Combine(currentPath, "category.json");
            if (!System.IO.File.Exists(categoriesPath))
            {
                return Ok(new { success = false, message = "Categories not found" });
            }

            List<Category> categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(categoriesPath));

            var filteredProducts = products.Where(p =>
                (string.IsNullOrEmpty(productName) || p.Name.ToLower().Contains(productName.ToLower()))
            )
            .Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name
            })
            .ToList();

            return Ok(new { success = true, data = filteredProducts });
        }


        [HttpGet]
        public IActionResult Get()
        {
            string categoriesPath = Path.Combine(currentPath, "category.json");
            if (!System.IO.File.Exists(categoriesPath))
            {
                return Ok(new { success = false, message = "Categories not found" });
            }

            List<Category> categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(categoriesPath));

            string productsPath = Path.Combine(currentPath, "products.json");
            if (!System.IO.File.Exists(productsPath))
            {
                return Ok(new { success = false, message = "Products not found" });
            }

            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath));

            var items = products.OrderByDescending(x => x.Id)
                               .Select(p => new
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Image = p.Image,
                                   Price = p.Price,
                                   CreatedAt = p.CreatedAt,
                                   CategoryId = p.CategoryId,
                                   CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name
                               })
                               .ToList();

            return Ok(new { success = true, data = items });
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategory(int? categoryId)
        {
            string categoriesPath = Path.Combine(currentPath, "category.json");
            if (!System.IO.File.Exists(categoriesPath))
            {
                return Ok(new { success = false, message = "Categories not found" });
            }

            List<Category> categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(categoriesPath));

            string productsPath = Path.Combine(currentPath, "products.json");
            if (!System.IO.File.Exists(productsPath))
            {
                return Ok(new { success = false, message = "Products not found" });
            }

            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath));

            var items = products.OrderByDescending(x => x.Id)
                               .Where(p => categoryId == null || p.CategoryId == categoryId)
                               .Select(p => new
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Image = p.Image,
                                   Price = p.Price,
                                   CreatedAt = p.CreatedAt,
                                   CategoryId = p.CategoryId,
                                   CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name
                               })
                               .ToList();

            return Ok(new { success = true, data = items });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string productsPath = currentPath + "products.json";
            if (!System.IO.File.Exists(productsPath))
            {
                return Ok(new { success = false, message = "Not found product" });
            }

            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath));
            var product = products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return Ok(new { success = false, message = "Product not found" });
            }

            string categoriesPath = currentPath + "category.json";
            if (!System.IO.File.Exists(categoriesPath))
            {
                return Ok(new { success = false, message = "Not found categories" });
            }

            List<Category> categories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(System.IO.File.ReadAllText(categoriesPath));
            var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);

            if (category == null)
            {
                return Ok(new { success = false, message = "Category not found" });
            }

            var result = new
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image,
                Price = product.Price,
                Description = product.Description,
                Status = product.Status,
                Brand = product.Brand,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                NameCategory = category.Name,
            };

            return Ok(new { success = true, data = result });
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            string path = currentPath + "products.json";
            List<Product> products = new List<Product>();
            if (!System.IO.File.Exists(path))
            {
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine("[]");
                }
            }
            var produtsJson = await System.IO.File.ReadAllTextAsync(path);
            products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(produtsJson);
            product.Id = ClickBuyHelper.AutoIncrement(products.OrderByDescending(x => x.Id).FirstOrDefault().Id);
            product.Description = product.Description;
            product.Price = product.Price;
            product.Image = product.Image;
            product.CategoryId = product.CategoryId;
            product.CreatedAt = DateTime.Now;
            product.Status = true;
            products.Add(product);
            string newJson = JsonConvert.SerializeObject(products);
            await System.IO.File.WriteAllTextAsync(path, newJson);
            return Ok(
                new { success = true, message = "Create success" }
            );

        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            string path = currentPath + "products.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(
                    new { success = false, message = "Not found product" }
                );
            }
            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(path));
            var productUpdate = products.FirstOrDefault(x => x.Id == id);
            if (productUpdate == null)
            {
                return Ok(
                    new { success = false, message = "Not found product" }
                );
            }
            productUpdate.Name = product.Name;
            productUpdate.Image = product.Image;
            productUpdate.CategoryId = product.CategoryId;
            productUpdate.Price = product.Price;
            productUpdate.Description = product.Description;

            string newJson = System.Text.Json.JsonSerializer.Serialize(products);
            System.IO.File.WriteAllText(path, newJson);
            return Ok(
                new { success = true, message = "Update success" }
            );

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string path = currentPath + "products.json";
            if (!System.IO.File.Exists(path))
            {
                return Ok(
                    new { success = false, message = "Not found product" }
                );
            }
            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(path));
            var productDelete = products.FirstOrDefault(x => x.Id == id);
            if (productDelete == null)
            {
                return Ok(
                    new { success = false, message = "Not found product" }
                );
            }
            products.Remove(productDelete);
            string newJson = System.Text.Json.JsonSerializer.Serialize(products);
            System.IO.File.WriteAllText(path, newJson);
            return Ok(
                new { success = true, message = "Delete success" }
            );
        }
    }
}
