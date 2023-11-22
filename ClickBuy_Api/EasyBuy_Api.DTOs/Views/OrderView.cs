using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Views
{
    public class OrderView : BaseView
    {
        public String? UserId { get; set; }
        public String? UserName { get; set; }
        public String? Quantity { get; set; }
        public String? PathImage { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
