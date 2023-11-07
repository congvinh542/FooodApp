using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.Huyen
{
    public interface IHuyenService : IBaseService<HuyenQuery, HuyenView, HuyenFilter>
    {
        Task<DataResult<HuyenView>> GetAll();   
        Task<DataResult<HuyenView>> GetByCodeAsync(string code);
    }
}
