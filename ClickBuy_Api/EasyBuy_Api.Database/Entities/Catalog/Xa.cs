using ClickBuy_Api.Database.Entities.Base;

namespace ClickBuy_Api.Database.Entities.Catalog
{
    public class Xa : BaseEntity
    {
        public Guid? HuyenId { get; set; }
        public Huyen? Huyen { get; set;}
    }
}
