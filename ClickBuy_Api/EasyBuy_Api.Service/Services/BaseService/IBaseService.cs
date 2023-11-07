using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Helpers;

namespace ClickBuy_Api.Service.Services.BaseService
{
    /// <summary>
    /// Base service interface
    /// </summary>
    /// <typeparam name="T1">Query</typeparam>
    /// <typeparam name="T2">View</typeparam>
    /// <typeparam name="T3">Filter</typeparam>
    public interface IBaseService<T1, T2, T3>
    {
        Task<DataResult<T2>> GetPageList(BaseFilter<T3> query);
        Task<DataResult<T2>> GetByIdAsync(string id);
        Task<DataResult<bool>> CreateAsync(T1 entity);
        Task<DataResult<int>> UpdateAsync(T1 entity, string id);
        Task<DataResult<int>> DeleteAsync(string id);
    }
}
