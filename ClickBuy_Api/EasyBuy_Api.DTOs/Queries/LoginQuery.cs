using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.DTOs.Queries
{
    public class LoginQuery
    {
        public String? Email { get; set; }
        public String? Password { get; set; }
        public Boolean? RememberMe { get; set; }
    }
}
