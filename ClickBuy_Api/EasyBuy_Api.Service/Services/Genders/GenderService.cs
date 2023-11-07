using ClickBuy_Api.Database.Entities;
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

namespace ClickBuy_Api.Service.Services.Genders
{
    public class GenderService : ServiceBase<IUnitOfWork>, IGenderService
    {
        private readonly IServiceProvider _serviceProvider;

        public GenderService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(GenderQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<GenderQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var gender = new Gender
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
            };
            await _unitOfWork.GetRepository<Gender>().Add(gender);
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
            var gender = await _unitOfWork.GetRepository<Gender>().GetByIdAsync(Guid.Parse(id));
            if (gender == null)
            {
                result.Errors.Add("Gender not found");
                return result;
            }
            _unitOfWork.GetRepository<Gender>().Delete(gender, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<GenderView>> GetByCodeAsync(string genderCode)
        {
            var result = new DataResult<GenderView>();
            var gender = await _unitOfWork.GetRepository<Gender>().AsQueryable().FirstOrDefaultAsync(x => x.Code == genderCode);
            if (gender == null)
            {
                result.Errors.Add("Gender not found");
                return result;
            }
            result.Entity = new GenderView
            {
                Id = gender.Id,
                Name = gender?.Name,
                Description = gender?.Description,
                Code = gender?.Code,
                
            };
            return result;
        }

        public async Task<DataResult<GenderView>> GetByIdAsync(string id)
        {
            var result = new DataResult<GenderView>();
            var Gender = await _unitOfWork.GetRepository<Gender>()
                .AsQueryable().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (Gender == null)
            {
                result.Errors.Add("Gender not found");
                return result;
            }
            result.Entity = new GenderView
            {
                Id = Gender.Id,
                Code = Gender.Code,
                Name = Gender.Name,
                Description = Gender.Description,
                CreatedBy = Gender.CreatedBy,
                CreatedAt = Gender.CreatedAt,
                UpdatedAt = Gender.UpdatedAt,
                IsActive = Gender.IsActive,
            };
            return result;
        }

        public async Task<DataResult<GenderView>> GetPageList(BaseFilter<GenderFilter> query)
        {
            var genders = await _unitOfWork.GetRepository<Gender>().AsQueryable()
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new GenderView()
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

            var response = new DataResult<GenderView>();
            response.TotalRecords = await _unitOfWork.GetRepository<Gender>().AsQueryable().CountAsync();
            response.Items = genders;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(GenderQuery entity, string id)
        {
            var result = new DataResult<int>();
            var gender = await _unitOfWork.GetRepository<Gender>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (gender == null)
            {
                result.Errors.Add("Gender not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<GenderQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            gender.Name = entity.Name;
            gender.Code = entity.Code;
            gender.Description = entity.Description;

            _unitOfWork.GetRepository<Gender>().Update(gender);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<GenderView>> GetAll()
        {
            var Gender = await _unitOfWork.GetRepository<Gender>().AsQueryable()
                      .Select(x => new GenderView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Code = x.Code,
                          Description = x.Description,
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<GenderView>();
            response.Items = Gender;
            return response;
        }
    }
}