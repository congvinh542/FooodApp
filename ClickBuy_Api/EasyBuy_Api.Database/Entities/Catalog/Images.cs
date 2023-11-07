using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Products;
using ClickBuy_Api.Database.Entities.System;

namespace ClickBuy_Api.Database.Entities.Catalog
{
    public class Images : BaseEntity
    {
        public String? FileName { get; set; }
        public String? FilePath { get; set; }
        public List<Product>? Products { get; set; }
        public List<Category>? Categorys { get; set; }
        public List<User>? Users { get; set; }
    }
}
