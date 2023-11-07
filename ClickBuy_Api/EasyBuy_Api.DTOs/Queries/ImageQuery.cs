using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Queries
{
    public class ImageQuery : BaseQueries
    {
        public String? FileName { get; set; }
        public String? FilePath { get; set; }
    }
}
