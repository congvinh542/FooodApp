using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Database.Entities.Rooms
{
    public class RoomDeatails : BaseEntity
    {
        public Guid? ImageId { get; set; }
        public Images? Images { get; set; }
        public int QuantityOfBed {  get; set; } 
    }
}
