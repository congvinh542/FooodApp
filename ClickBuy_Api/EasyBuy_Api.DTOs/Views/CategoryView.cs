using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Views
{
    public class CategoryView : BaseView
    {
        public Guid? ParentCategoryId { set; get; }
        public Guid? ImageId { set; get; }
        public String? PathImage { set; get; }
    }
}
