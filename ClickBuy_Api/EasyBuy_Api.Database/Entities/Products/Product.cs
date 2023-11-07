using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Database.Entities.Products
{
    public class Product : BaseEntity
    {
        public decimal Price { get; set; } = 0;
        public int? Quantity { get; set; }
        public Boolean? IsHot { get; set; } = false;

        public Guid? CategoryId { get; set; }
        public Category? Categorys { get; set; }

        public Guid? ImageId { get; set; }
        public Images? Images { get; set; }
    }
}
