using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.OrderDetails
{
    public interface IOrderDetailsService : IBaseService<OrderDetailQuery, OrderDetailView, OrderDetailsFilter>
    {
        Task<DataResult<OrderDetailView>> GetByCodeAsync(string code);
        Task<DataResult<OrderDetailView>> GetAll();
    }
}
