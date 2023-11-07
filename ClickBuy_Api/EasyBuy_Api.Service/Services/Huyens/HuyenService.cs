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

namespace ClickBuy_Api.Service.Services.Huyen
{
    public class HuyenService : ServiceBase<IUnitOfWork>, IHuyenService
    {
        private readonly IServiceProvider _serviceProvider;

        public HuyenService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(HuyenQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<HuyenQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var huyen = new ClickBuy_Api.Database.Entities.Catalog.Huyen
            {
                Name = entity.Name,
                Description = entity.Description,
                TinhId = Guid.Parse(entity.TinhId)
            };
            await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().Add(huyen);
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
            var huyen = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().GetByIdAsync(Guid.Parse(id));
            if (huyen == null)
            {
                result.Errors.Add("Huyen not found");
                return result;
            }
            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().Delete(huyen, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<HuyenView>> GetByCodeAsync(string HuyenCode)
        {
            var result = new DataResult<HuyenView>();
            var huyen = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().AsQueryable().FirstOrDefaultAsync(x => x.Code == HuyenCode);
            if (huyen == null)
            {
                result.Errors.Add("Huyen not found");
                return result;
            }
            result.Entity = new HuyenView
            {
                Id = huyen.Id,
                Name = huyen.Name,
                Code = huyen.Code,
                Description = huyen.Description,
                TinhId = huyen.TinhId.ToString(),
                TinhName = huyen.Tinh.Name
            };
            return result;
        }

        public async Task<DataResult<HuyenView>> GetByIdAsync(string id)
        {
            var result = new DataResult<HuyenView>();
            var huyen = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>()
                .AsQueryable().Include(x => x.Tinh).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (huyen == null)
            {
                result.Errors.Add("Huyen not found");
                return result;
            }
            result.Entity = new HuyenView
            {
                Id = huyen.Id,
                Code = huyen.Code,
                Name = huyen.Name,
                Description = huyen.Description,
                TinhId = huyen.TinhId.ToString(),
                TinhName = huyen.Tinh?.Name,

                CreatedBy = huyen.CreatedBy,
                CreatedAt = huyen.CreatedAt,
                UpdatedAt = huyen.UpdatedAt,
                IsActive = huyen.IsActive,
            };
            return result;
        }

        public async Task<DataResult<HuyenView>> GetPageList(BaseFilter<HuyenFilter> query)
        {
            var Huyens = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().AsQueryable()
                     .Include(x => x.Tinh)
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new HuyenView()
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Code = x.Code,
                         Description = x.Description,
                         TinhId = x.TinhId.ToString(),
                         TinhName = x.Tinh.Name,

                         CreatedBy = x.CreatedBy,
                         CreatedAt = x.CreatedAt,
                         UpdatedAt = x.UpdatedAt,
                         IsActive = x.IsActive,
                     })
                     .ApplyFilter(query)
                     .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<HuyenView>();
            response.TotalRecords = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().AsQueryable().CountAsync();
            response.Items = Huyens;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(HuyenQuery entity, string id)
        {
            var result = new DataResult<int>();
            var huyen = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (huyen == null)
            {
                result.Errors.Add("Huyen not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<HuyenQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            huyen.Name = entity.Name;
            huyen.Code = entity.Code;
            huyen.Description = entity.Description;
            huyen.TinhId = Guid.Parse(entity.TinhId);

            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().Update(huyen);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<HuyenView>> GetAll()
        {
            var Huyen = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Huyen>().AsQueryable()
                      .Include(x => x.Tinh)
                      .Select(x => new HuyenView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Code = x.Code,
                          Description = x.Description,
                          TinhId = x.TinhId.ToString(),
                          TinhName = x.Tinh.Name,
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<HuyenView>();
            response.Items = Huyen;
            return response;
        }
    }
}