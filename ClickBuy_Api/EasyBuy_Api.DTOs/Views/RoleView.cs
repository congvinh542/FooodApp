using ClickBuy_Api.Database.Entities.Base;

namespace ClickBuy_Api.DTOs.Views
{
    public class RoleView : BaseEntity
    {
        public String? RoleName { get; set; }
        public List<PermissionView>? Permissions { get; set; }

    }
}