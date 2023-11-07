namespace ClickBuy_Api.Service.Services.BaseService
{
    public class ServiceBase<T>
    {
        protected readonly T _unitOfWork;
        public ServiceBase(T unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
