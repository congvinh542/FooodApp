using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.SDK.Extensions;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using ClickBuy_Api.Service.Validator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Services.Xas
{
    public class XaService : ServiceBase<IUnitOfWork>, IXaService
    {
        private readonly IServiceProvider _serviceProvider;

        public XaService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(XaQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<XaQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var xa = new ClickBuy_Api.Database.Entities.Catalog.Xa
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                HuyenId = Guid.Parse(entity.HuyenId)
            };
            await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().Add(xa);
            result.Entity = await _unitOfWork.SaveChangesAsync() > 0;
            if (result.Entity == false)
            {
                result.Errors.Add("Error while saving");
                return result;
            }
            return result;
        }

        public async Task<DataResult<int>> DeleteAsync(string id)
        {
            var result = new DataResult<int>();
            var xa = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().GetByIdAsync(Guid.Parse(id));
            if (xa == null)
            {
                result.Errors.Add("Xa not found");
                return result;
            }
            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().Delete(xa, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<XaView>> GetByCodeAsync(string XaCode)
        {
            var result = new DataResult<XaView>();
            var xa = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().AsQueryable().FirstOrDefaultAsync(x => x.Code == XaCode);
            if (xa == null)
            {
                result.Errors.Add("Xa not found");
                return result;
            }
            result.Entity = new XaView
            {
                Id = xa.Id,
                Name = xa.Name,
                Description = xa.Description,
                HuyenId = xa.HuyenId.ToString(),
                HuyenName = xa.Huyen.Name
            };
            return result;
        }

        public async Task<DataResult<XaView>> GetByIdAsync(string id)
        {
            var result = new DataResult<XaView>();
            var xa = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>()
                .AsQueryable().Include(x => x.Huyen).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (xa == null)
            {
                result.Errors.Add("Xa not found");
                return result;
            }
            result.Entity = new XaView
            {
                Id = xa.Id,
                Code = xa.Code,
                Name = xa.Name,
                Description = xa.Description,
                HuyenId = xa.HuyenId.ToString(),
                HuyenName = xa.Huyen?.Name,

                CreatedBy = xa.CreatedBy,
                CreatedAt = xa.CreatedAt,
                UpdatedAt = xa.UpdatedAt,
                IsActive = xa.IsActive,
            };
            return result;
        }

        public async Task<DataResult<XaView>> GetPageList(BaseFilter<XaFilter> query)
        {
            var Xas = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().AsQueryable()
                     .Include(x => x.Huyen)
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new XaView()
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Code = x.Code,
                         Description = x.Description,
                         HuyenId = x.HuyenId.ToString(),
                         HuyenName = x.Huyen.Name,

                         CreatedBy = x.CreatedBy,
                         CreatedAt = x.CreatedAt,
                         UpdatedAt = x.UpdatedAt,
                         IsActive = x.IsActive,
                     })
                     .ApplyFilter(query)
                     .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<XaView>();
            response.TotalRecords = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().AsQueryable().CountAsync();
            response.Items = Xas;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(XaQuery entity, string id)
        {
            var result = new DataResult<int>();
            var xa = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (xa == null)
            {
                result.Errors.Add("Xa not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<XaQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            xa.Name = entity.Name;
            xa.Description = entity.Description;
            xa.HuyenId = Guid.Parse(entity.HuyenId);

            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().Update(xa);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<XaView>> GetAll()
        {
            var xa = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Xa>().AsQueryable()
                      .Include(x => x.Huyen)
                      .Select(x => new XaView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Description = x.Description,
                          HuyenId = x.HuyenId.ToString(),
                          HuyenName = x.Huyen.Name,
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<XaView>();
            response.Items = xa;
            return response;
        }
    }
}