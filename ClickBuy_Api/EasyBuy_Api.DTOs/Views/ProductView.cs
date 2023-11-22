using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Views
{
    public class ProductView : BaseView
    {
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public String? PathImage { get; set; }
        public String? PathCategory { get; set; }
        public String? CodeCategory { get; set; }
        public String? NameCategory { get; set; }

        // Foreign key
        public String? ImageId { get; set; }
        public String? CategoryId { get; set; }

    }
}
