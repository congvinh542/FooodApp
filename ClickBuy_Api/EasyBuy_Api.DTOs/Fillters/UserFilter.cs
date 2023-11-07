using ClickBuy_Api.DTOs.Models;

namespace ClickBuy_Api.DTOs.Fillters
{
    public class UserFilter
    {
        public String? Email { get; set; }
        public String? Username { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public Boolean? IsActive { get; set; }
        public RangeFilter<DateTime?>? DateOfBirth { get; set; }
        public RangeFilter<DateTime?>? CreatedAt { get; set; }
        public RangeFilter<DateTime?>? UpdatedAt { get; set; }
    }
}