using ClickBuy_Api.DTOs.Models;

namespace ClickBuy_Api.DTOs.Fillters
{
    public class PermissionFilter 
    {
        public Guid? RoleId { get; set; }
        public Guid? PermissionId { get; set; }
        public String? RoleName { get; set; }
        public String? PermissionKey { get; set; }
        public String? PermissionValue { get; set; }
        public String? Description { get; set; }

        public Boolean? IsActive { get; set; }
        public RangeFilter<DateTime?>? CreatedAt { get; set; }
        public RangeFilter<DateTime?>? UpdatedAt { get; set; }

    }
}