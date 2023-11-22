using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Queries
{
    public class OrderQuery : BaseQueries
    {
        public String? PathImage { get; set; }
        public String? TotalAmount { get; set; }
        public String? Quantity { get; set; }
    }
}
