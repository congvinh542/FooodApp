using ClickBuy_Api.Database.Entities.Base;
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
        public Guid UserId { get; set; } // Khóa ngoại đến lớp User
        public User User { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        // Một đơn hàng có nhiều chi tiết đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
