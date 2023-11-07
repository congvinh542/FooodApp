using ClickBuy_Api.Database.Entities.Base;

namespace ClickBuy_Api.Database.Entities.Catalog
{
    public class Huyen : BaseEntity
    {
        public Guid? TinhId { get; set; }
        public Tinh? Tinh { get; set;}
        public List<Xa>? Xas { get; set; }
    }
}
