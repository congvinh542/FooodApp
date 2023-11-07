using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;

namespace ClickBuy_Api.Service.Services.Orders
{
    public interface IOrderService : IBaseService<OrderQuery, OrderView, OrderFilter>
    {
        Task<DataResult<OrderView>> GetByCodeAsync(string code);
        Task<DataResult<OrderView>> GetAll();
    }
}
