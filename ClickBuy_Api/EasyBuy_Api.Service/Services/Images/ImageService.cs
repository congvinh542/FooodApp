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

namespace ClickBuy_Api.Service.Services.Images
{
    public class ImageService : ServiceBase<IUnitOfWork>, IimageService
    {
        private readonly IServiceProvider _serviceProvider;


        public ImageService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(ImageQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<ImageQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var image = new ClickBuy_Api.Database.Entities.Catalog.Images
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                FileName = entity.FileName,
                FilePath = entity.FilePath,
                CreatedAt = DateTime.Now,

            };
            await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().Add(image);
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
            var image = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().GetByIdAsync(Guid.Parse(id));
            if (image == null)
            {
                result.Errors.Add("Image not found");
                return result;
            }
            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().Delete(image, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<ImageView>> GetByCodeAsync(string ImageCode)
        {
            var result = new DataResult<ImageView>();
            var image = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable().FirstOrDefaultAsync(x => x.Code == ImageCode);
            if (image == null)
            {
                result.Errors.Add("Image not found");
                return result;
            }
            result.Entity = new ImageView
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                FileName = image.FileName,
                FilePath = image.FilePath,
            };
            return result;
        }

        public async Task<DataResult<ImageView>> GetByIdAsync(string id)
        {
            var result = new DataResult<ImageView>();
            var image = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>()
                .AsQueryable().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (image == null)
            {
                result.Errors.Add("Image not found");
                return result;
            }
            result.Entity = new ImageView
            {
                Id = image.Id,
                Code = image?.Code,
                Name = image?.Name,
                Description = image?.Description,
                FilePath = image?.FilePath,
                FileName = image?.FileName,

                CreatedBy = image?.CreatedBy,
                CreatedAt = image?.CreatedAt,
                UpdatedAt = image?.UpdatedAt,
                IsActive = image?.IsActive,
            };
            return result;
        }

        public async Task<DataResult<ImageView>> GetPageList(BaseFilter<ImageFilter> query)
        {
            var images = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable()
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new ImageView()
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Code = x.Code,
                         Description = x.Description,
                         FileName = x.FileName,
                         FilePath = x.FilePath,

                         CreatedBy = x.CreatedBy,
                         CreatedAt = x.CreatedAt,
                         UpdatedAt = x.UpdatedAt,
                         IsActive = x.IsActive,
                     })
                     .ApplyFilter(query)
                     .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<ImageView>();
            response.TotalRecords = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable().CountAsync();
            response.Items = images;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(ImageQuery entity, string id)
        {
            var result = new DataResult<int>();
            var image = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (image == null)
            {
                result.Errors.Add("Image not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<ImageQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            image.Name = entity.Name;
            image.Code = entity.Code;
            image.FileName = entity.FileName;
            image.FilePath = entity.FilePath;
            image.Description = entity.Description;

            _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().Update(image);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<ImageView>> GetAll()
        {
            var image = await _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable()
                      .Select(x => new ImageView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Description = x.Description,
                          Code = x.Code,
                          FileName = x.FileName,
                          FilePath = x.FilePath,
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<ImageView>();
            response.Items = image;
            return response;
        }
    }
}
