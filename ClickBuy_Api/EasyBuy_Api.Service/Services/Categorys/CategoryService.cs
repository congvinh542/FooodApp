using ClickBuy_Api.Database.Entities.Catalog;
using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.SDK.Extensions;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using ClickBuy_Api.Service.Services.Images;
using ClickBuy_Api.Service.Validator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ClickBuy_Api.Service.Services.Categorys
{
    public class CategoryService : ServiceBase<IUnitOfWork>, ICategoryService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IimageService _imageService;

        public CategoryService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IimageService iimageService) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _imageService = iimageService;

        }

        public async Task<DataResult<bool>> CreateAsync(CategoryQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<CategoryQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var category = new Category
            {
                Name = entity.Name,
                Code = entity.Code,
                ImageId = entity.Id,
                Description = entity.Description,
            };
            await _unitOfWork.GetRepository<Category>().Add(category);
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
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(Guid.Parse(id));
            if (category == null)
            {
                result.Errors.Add("Permission not found");
                return result;
            }
            _unitOfWork.GetRepository<Category>().Delete(category, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<CategoryView>> GetByIdAsync(string id)
        {
            var result = new DataResult<CategoryView>();
            var category = await _unitOfWork.GetRepository<Category>()
                .AsQueryable().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (category == null)
            {
                result.Errors.Add("Category not found");
                return result;
            }
            result.Entity = new CategoryView
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                ImageId = category.ImageId,
                Description = category.Description,

                CreatedBy = category.CreatedBy,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                IsActive = category.IsActive,
            };
            return result;
        }

        public async Task<DataResult<CategoryView>> GetPageList(BaseFilter<CategoryFilter> query)
        {
            var category = await _unitOfWork.GetRepository<Category>().AsQueryable()
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new CategoryView()
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Code = x.Code,
                         Description = x.Description,
                         ImageId = x.ImageId,
                         PathImage = _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable()
                                      .Where(image => image.Id == x.ImageId)
                                      .Select(image => image.FilePath) // Lấy đường dẫn hình ảnh từ ImageId
                                      .FirstOrDefault(),
                         CreatedBy = x.CreatedBy,
                         CreatedAt = x.CreatedAt,
                         UpdatedAt = x.UpdatedAt,
                         IsActive = x.IsActive,
                     })
                     .ApplyFilter(query)
                     .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<CategoryView>();
            response.TotalRecords = await _unitOfWork.GetRepository<Category>().AsQueryable().CountAsync();
            response.Items = category;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(CategoryQuery entity, string id)
        {
            var result = new DataResult<int>();
            var category = await _unitOfWork.GetRepository<Category>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (category == null)
            {
                result.Errors.Add("Category not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<CategoryQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            category.Name = entity.Name;

            category.Description = entity.Description;

            _unitOfWork.GetRepository<Category>().Update(category);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<CategoryView>> GetAll()
        {
            var categories = await _unitOfWork.GetRepository<Category>().AsQueryable()
                      .Select(x => new CategoryView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Code = x.Code,
                          Description = x.Description,
                          ImageId = x.ImageId,
                          PathImage = _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable()
                                      .Where(image => image.Id == x.ImageId)
                                      .Select(image => image.FilePath) // Lấy đường dẫn hình ảnh từ ImageId
                                      .FirstOrDefault(),
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();

            var response = new DataResult<CategoryView>();
            response.Items = categories;
            return response;
        }

        public async Task<DataResult<CategoryView>> GetByCodeAsync(string code)
        {
            var result = new DataResult<CategoryView>();
            var category = await _unitOfWork.GetRepository<Category>().AsQueryable().FirstOrDefaultAsync(x => x.Code == code);
            if (category == null)
            {
                result.Errors.Add("Category not found");
                return result;
            }
            result.Entity = new CategoryView
            {
                Id = category.Id,
                Name= category.Name,
                Code = category.Code,
                Description = category.Description,
            };
            return result;
        }
    }
}