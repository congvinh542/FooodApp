using ClickBuy_Api.Database.Common;

namespace ClickBuy_Api.Database.Entities.Base
{
    public class BaseEntity
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public String? Code { get; set; } = HelperCommon.GenerateCode(8, "#");
        public String? Name { get; set; } = null;
        public String? Description { get; set; } = null;
		public String? CreatedBy { get; set; } = "System";
        public String? UpdatedBy { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public Boolean? IsActive { get; set; } = true;
        public Boolean? IsDeleted { get; set;} = false;
        public Boolean? IsDefault { get; set; } = true; 
    }
}
