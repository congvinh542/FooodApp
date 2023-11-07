using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.Categorys
{
    public interface ICategoryService : IBaseService<CategoryQuery, CategoryView, CategoryFilter>
    {
        Task<DataResult<CategoryView>> GetByCodeAsync(string code);
        Task<DataResult<CategoryView>> GetAll();
    }
}
