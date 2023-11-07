using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Helpers;

namespace ClickBuy_Api.Service.MailServices
{
    public interface IMailService
    {
        Task<DataResult<bool>> SendEmailAsync(MailQuery mailRequest);
    }
}
