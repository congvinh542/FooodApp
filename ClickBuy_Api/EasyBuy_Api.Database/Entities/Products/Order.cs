using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Catalog;
using ClickBuy_Api.Database.Entities.Products;
using ClickBuy_Api.Database.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Database.Entities
{
    public class Order : BaseEntity
    {
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public String? Quantity { get; set; }
        public String? PathImage { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
