using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Services.Tinhs
{
    public interface ITinhService : IBaseService<TinhQuery, TinhView, TinhFilter>
    {
        Task<DataResult<TinhView>> GetByCodeAsync(string code);
        Task<DataResult<TinhView>> GetAll();

    }
}
