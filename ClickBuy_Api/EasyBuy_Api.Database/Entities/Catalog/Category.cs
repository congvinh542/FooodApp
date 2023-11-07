using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Products;

namespace ClickBuy_Api.Database.Entities.Catalog
{
    public class Category : BaseEntity
    {
        public Guid? ImageId { get; set; }
        public Images? Images { get; set; }
        public List<Product>? Products { get; set; }
    }
}
