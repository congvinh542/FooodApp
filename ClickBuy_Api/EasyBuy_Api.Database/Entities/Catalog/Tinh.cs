using ClickBuy_Api.Database.Entities.Base;

namespace ClickBuy_Api.Database.Entities.Catalog
{
    public class Tinh : BaseEntity
    {
        public List<Huyen>? Huyens { get; set; }
    }
}
