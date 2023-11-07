namespace ClickBuy_Api.DTOs.Queries
{
    public class RegisterQuery
    {
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? Email { get; set; }
        public String? Password { get; set; }
        public String? ComfirmPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
