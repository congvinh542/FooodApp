using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.System;

namespace ClickBuy_Api.Database.Entities
{
    public class Gender : BaseEntity
    {
        public List<User>? Users { get; set; }
    }
}
