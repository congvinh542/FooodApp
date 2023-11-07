using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Fillters
{
    public class CategoryFilter : BaseFilter<CategoryFilter>
    {
        public String? Name { get; set; }
        public String? Code { get; set; }
    }
}
