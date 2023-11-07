namespace ClickBuy_Api.DTOs.Queries
{
    public class UserQuery
    {
        public String? ResetCode { get; set; }
        public String? Email { get; set; }
        public String? UserName { get; set; } 
        public String? Password { get; set; }  
        public String? FirstName { get; set; }
        public String? Phone { get; set; }
        public String? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? TinhId { get; set; }
        public Guid? HuyenId { get; set; }
        public Guid? XaId { get; set; }
        public Guid? GenderId { get; set; }

        public String? LastName { get; set; } 
        public String[]? Role { get; set; } 
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
        public DateTime? LastActive { get; set; } 
    }
}
