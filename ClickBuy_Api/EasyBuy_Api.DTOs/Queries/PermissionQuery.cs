using ClickBuy_Api.Database.Entities.Base;

namespace ClickBuy_Api.DTOs.Queries
{
    public class PermissionQuery : BaseEntity
    {
        public Guid? RoleId { get; set; }
        public Guid? PermissionId { get; set; }
        public String? PermissionKey { get; set; }
        public String? PermissionValue { get; set; }
    }
}