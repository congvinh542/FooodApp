using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Database.Entities.Products
{
    public class OrderDetail : BaseEntity
    {
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }   

        public Guid OrderId { get; set; } // Khóa ngoại đến lớp Order
        public Order Order { get; set; }

        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
