using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.DTOs.Queries;

namespace ClickBuy_Api.Service.Services.TokenServices
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
