namespace ClickBuy_Api.Service.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<int> GetAll(string dashboardId);
    }
}
