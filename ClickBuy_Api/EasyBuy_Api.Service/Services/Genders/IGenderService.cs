using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.Genders
{
    public interface IGenderService : IBaseService<GenderQuery, GenderView, GenderFilter>
    {
        Task<DataResult<GenderView>> GetAll();
        Task<DataResult<GenderView>> GetByCodeAsync(string genderCode);
    }
}
