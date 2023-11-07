using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.Images
{
    public interface IimageService : IBaseService<ImageQuery, ImageView, ImageFilter>
    {
        Task<DataResult<ImageView>> GetAll();
        Task<DataResult<ImageView>> GetByCodeAsync(string imageCode);
    }
}
