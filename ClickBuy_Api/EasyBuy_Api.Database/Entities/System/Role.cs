using Microsoft.AspNetCore.Identity;

namespace ClickBuy_Api.Database.Entities.System
{
    public class Role : IdentityRole<Guid>
    {
        public List<UserRole>? UserRoles { get; set; }
        public List<RolePermission>? RolePermissions { get; set; }

    }
}
