using ClickBuy_Api.Common;
using ClickBuy_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClickBuy_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        string currentPath = Directory.GetCurrentDirectory() + "\\Data\\";
        private readonly IConfiguration _configuration;
        private List<User> _users;
        private readonly string _currentPath;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _currentPath = Directory.GetCurrentDirectory() + "\\Data\\";
            _users = GetUsers();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string path = currentPath + "users.json";
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                var user = users.FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return Ok(new { success = false });
                }
                return Ok(
                    new { success = true, data = user }
                );
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = AuthenticateUser(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(user);
            return Ok(new
            {
                success = true,
                data = user,
                token = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            string path = currentPath + "users.json";
            List<User> users = new List<User>();
            if (!System.IO.File.Exists(path))
            {
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine("[]");
                }
            }
            var registersJson = await System.IO.File.ReadAllTextAsync(path);
            users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(registersJson);

            var user = new User
            {
                Id = ClickBuyHelper.AutoIncrement(users.OrderByDescending(x => x.Id).FirstOrDefault().Id),
                Username = model.Username,
                Password = model.Password,
                Role = "Admin",
            };

            users.Add(user);
            string newJson = JsonConvert.SerializeObject(users);
            await System.IO.File.WriteAllTextAsync(path, newJson);
            return Ok(
                new { success = true, message = "Register success" }
            );
        }

        private User AuthenticateUser(string username, string password)
        {
            return _users.Find(u => u.Username == username && u.Password == password);
        }

        private List<User> GetUsers()
        {
            string path = _currentPath + "users.json";
            var users = new List<User>();

            if (!System.IO.File.Exists(path))
            {
                users.Add(new User { Id = 1, Username = "admin", Password = "admin", Role = "admin" });
                users.Add(new User { Id = 2, Username = "mod", Password = "mod", Role = "mod" });
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(users));
            }
            else
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    users = JsonConvert.DeserializeObject<List<User>>(json);
                }
            }

            return users;
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
