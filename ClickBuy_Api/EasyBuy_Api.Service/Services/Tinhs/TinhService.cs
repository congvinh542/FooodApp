using ClickBuy_Api.Database.Entities.Catalog;
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
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Services.Tinhs
{
    public class TinhService : ServiceBase<IUnitOfWork>, ITinhService
    {
        private readonly IServiceProvider _serviceProvider;

        public TinhService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(TinhQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<TinhQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var tinh = new ClickBuy_Api.Database.Entities.Catalog.Tinh
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                CreatedAt = DateTime.Now,
            };
            await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().Add(tinh);
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
            var tinh = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().GetByIdAsync(Guid.Parse(id));
            if (tinh == null)
            {
                result.Errors.Add("Conscious not found");
                return result;
            }
            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().Delete(tinh, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<TinhView>> GetByCodeAsync(string TinhCode)
        {
            var result = new DataResult<TinhView>();
            var Tinh = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().AsQueryable().FirstOrDefaultAsync(x => x.Code == TinhCode);
            if (Tinh == null)
            {
                result.Errors.Add("Tinh not found");
                return result;
            }
            result.Entity = new TinhView
            {
                Id = Tinh.Id,
                Name = Tinh.Name,
                Code = Tinh.Code,
            };
            return result;
        }

        public async Task<DataResult<TinhView>> GetByIdAsync(string id)
        {
            var result = new DataResult<TinhView>();
            var tinh = await _unitOfWork.GetRepository<Tinh>()
                .AsQueryable().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (tinh == null)
            {
                result.Errors.Add("Tinh not found");
                return result;
            }
            result.Entity = new TinhView
            {
                Id = tinh.Id,
                Code = tinh.Code,
                Name = tinh.Name,
                CreatedBy = tinh.CreatedBy,
                CreatedAt = tinh.CreatedAt,
                UpdatedAt = tinh.UpdatedAt,
                IsActive = tinh.IsActive,
            };
            return result;
        }

        public async Task<DataResult<TinhView>> GetPageList(BaseFilter<TinhFilter> query)
        {
            var Tinhs = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().AsQueryable()
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new TinhView()
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Code = x.Code,
                         Description = x.Description,

                         CreatedBy = x.CreatedBy,
                         CreatedAt = x.CreatedAt,
                         UpdatedAt = x.UpdatedAt,
                         IsActive = x.IsActive,
                     })
                     .ApplyFilter(query)
                     .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<TinhView>();
            response.TotalRecords = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().AsQueryable().CountAsync();
            response.Items = Tinhs;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(TinhQuery entity, string id)
        {
            var result = new DataResult<int>();
            var Tinh = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (Tinh == null)
            {
                result.Errors.Add("Tinh not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<TinhQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            Tinh.Name = entity.Name;
            Tinh.Description = entity.Description;

            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().Update(Tinh);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<TinhView>> GetAll()
         {
            var tinh = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Tinh>().AsQueryable()
                      .Select(x => new TinhView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Description = x.Description,
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<TinhView>();
            response.Items = tinh;
            return response;
        }
    }
}
