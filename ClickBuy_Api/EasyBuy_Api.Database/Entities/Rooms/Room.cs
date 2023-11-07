using ClickBuy_Api.Database.Entities.Base;
using ClickBuy_Api.Database.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Database.Entities.Rooms
{
    public class Room : BaseEntity
    {
        public int Price { get; set; }
        public int Quantity {  get; set; }
        public bool TypeRoom { get; set; }
        public Guid? ImageId { get; set; }
        public Images? Images { get; set; }
        public Guid? RoomDetailsId { get; set; }
        public RoomDeatails? RoomDetails {  get; set; }
    }
}
