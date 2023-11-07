using ClickBuy_Api.Database.Entities.Catalog;
using Microsoft.AspNetCore.Identity;

namespace ClickBuy_Api.Database.Entities.System
{
    public class User : IdentityUser<Guid>
    {
        public String? ResetCode { get; set; } = null; // mã quên mật khẩu 
        public String? Otp { get; set; } = null; // Mã đăng nhập hệ thống

        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? Phone { get; set; }
        public String? Address { get; set; }
        public DateTime DateOfBirth { get; set; }

        //Customer
        public int? QuantityProduct { get; set; } = null; // tổng số sản phẩm đã mua
        public decimal? TotalMoney { get; set; } = null; //tổng số tiền đã giao dịch
        // Foreign key
        public List<RolePermission>? UserPermissions { get; set; }
        public Guid? ImageId { get; set; } = null;
        public Images? Image { get; set; }
        public Guid? TinhId { get; set; } = null;
        public Tinh? Tinh { get; set; }
        public Guid? HuyenId { get; set; } = null;
        public Huyen? Huyen { get; set; }
        public Guid? XaId { get; set; } = null;
        public Xa? Xa { get; set; }
        public Guid? GenderId { get; set; } = null;
        public Gender? Gender { get; set; }
        public List<Order> Orders { get; set; }

        // Base
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
