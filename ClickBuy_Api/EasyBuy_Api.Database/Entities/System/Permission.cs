using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.System;

namespace ClickBuy_Api.Database.Entities
{
    public class Permission : BaseEntity
    {
        public String? Key { get; set; }
        public String? Value { get; set; }

        // Foreign key
        public List<RolePermission>? RolePermissions { get; set; }
    }
}
