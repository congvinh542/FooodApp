using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.Products
{
    public interface IProductService : IBaseService<ProductQuery, ProductView, ProductFilter>
    {
        Task<DataResult<ProductView>> GetByCodeAsync(string code);
        Task<DataResult<ProductView>> GetAll();
        Task<DataResult<ProductView>> GetDrinksAsync();
        Task<DataResult<ProductView>> GetBestSellersAsync();
    }
}
