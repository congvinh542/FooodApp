using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Queries
{
    public class ProductQuery : BaseQueries
    {
        public decimal Price { get; set; }
        public int? Quantity { get; set; }

        // Foreign key
        public String? ImageId { get; set; }
        public String? CategoryId { get; set; }
    }
}
