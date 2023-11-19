using ClickBuy_Api.Database.Entities.Products;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ClickBuy_Api.Service.Services.Products
{
    public class ProductService : ServiceBase<IUnitOfWork>, IProductService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProductService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(ProductQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<ProductQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var product = new Product
            {
                Name = entity.Name,
                Code = entity.Code,
                Price = entity.Price,
                Quantity = entity.Quantity,
                Description = entity.Description,
                ImageId = Guid.Parse(entity.ImageId),
                CategoryId = Guid.Parse(entity.CategoryId)
            };
            await _unitOfWork.GetRepository<Product>().Add(product);
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
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(id));
            if (product == null)
            {
                result.Errors.Add("Product not found");
                return result;
            }
            _unitOfWork.GetRepository<Product>().Delete(product, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<ProductView>> GetByCodeAsync(string code)
        {
            var result = new DataResult<ProductView>();
            var product = await _unitOfWork.GetRepository<Product>().AsQueryable().FirstOrDefaultAsync(x => x.Code == code);
            if (product == null)
            {
                result.Errors.Add("Product not found");
                return result;
            }
            result.Entity = new ProductView
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,

                ImageId = product.ImageId.ToString(),
                CategoryId = product.CategoryId.ToString(),
                pathImage = product.Images.FilePath,
            };
            return result;
        }

        public async Task<DataResult<ProductView>> GetByIdAsync(string id)
        {
            var result = new DataResult<ProductView>();
            var product = await _unitOfWork.GetRepository<Product>()
                .AsQueryable()
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (product == null)
            {
                result.Errors.Add("Product not found");
                return result;
            }
            result.Entity = new ProductView
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageId = product.ImageId.ToString(),
                CategoryId = product.CategoryId.ToString(),
                pathImage = product.Images?.FilePath,

                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                IsActive = product.IsActive,
            };
            return result;
        }

        public async Task<DataResult<List<ProductView>>> GetProductsByCategory(string id)
        {
            var result = new DataResult<List<ProductView>>();
            var products = await _unitOfWork.GetRepository<Product>()
                .AsQueryable()
                .Include(x => x.Images)
                .Include(x => x.Categorys)
                .Where(x => x.CategoryId == Guid.Parse(id))
                .ToListAsync();

            if (products == null || !products.Any())
            {
                result.Errors.Add("No products found for the given category");
                return result;
            }

            result.Entity = products.Select(product => new ProductView
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageId = product.ImageId.ToString(),
                CategoryId = product.CategoryId.ToString(),
                pathImage = _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Images>().AsQueryable()
                                      .Where(image => image.Id == product.ImageId)
                                      .Select(image => image.FilePath)
                                      .FirstOrDefault(),
                NameCategory = _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.Catalog.Category>().AsQueryable()
                                      .Where(name => name.Id == product.CategoryId)
                                      .Select(name => name.Name)
                                      .FirstOrDefault(),
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                IsActive = product.IsActive,
            }).ToList();

            return result;
        }


        public async Task<DataResult<ProductView>> GetPageList(BaseFilter<ProductFilter> query)
        {
            // Kiểm tra xem có điều kiện lọc theo categoryId không
            if (query.Entity != null && query.Entity.CategoryId != null)
            {
                var categoryId = query.Entity.CategoryId; // Lấy categoryId từ query.Entity

                var products = await _unitOfWork.GetRepository<Product>().AsQueryable()
                    .Include(x => x.Images)
                    .Include(x => x.Categorys)
                   
                    .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                    .Take(query.PageSize.Value)
                    .Select(x => new ProductView()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        CategoryId = x.CategoryId.ToString(),
                        NameCategory = x.Categorys.Name,
                        ImageId = x.ImageId.ToString(),
                        pathImage = x.Images.FilePath,
                        CreatedBy = x.CreatedBy,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        IsActive = x.IsActive,
                    })
                    .ApplyFilter(query)
                    .OrderByColums(query.SortColums, true).ToListAsync();

                var response = new DataResult<ProductView>();
         

                response.TotalRecords = await _unitOfWork.GetRepository<Product>().AsQueryable().CountAsync();
                response.Items = products;

                return response;
            }
            else
            {
                // Nếu không có điều kiện lọc, bạn có thể giữ nguyên phần cũ
                var products = await _unitOfWork.GetRepository<Product>().AsQueryable()
                    .Include(x => x.Categorys)
                    .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                    .Take(query.PageSize.Value)
                    .Select(x => new ProductView()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        CategoryId = x.CategoryId.ToString(),
                        ImageId = x.ImageId.ToString(),
                        pathImage = x.Images.FilePath,
                        CreatedBy = x.CreatedBy,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        IsActive = x.IsActive,
                    })
                    .ApplyFilter(query)
                    .OrderByColums(query.SortColums, true).ToListAsync();

                var response = new DataResult<ProductView>();
                response.Items = products;

                return response;
            }
        }


        public async Task<DataResult<int>> UpdateAsync(ProductQuery entity, string id)
        {
            var result = new DataResult<int>();
            var products = await _unitOfWork.GetRepository<Product>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (products == null)
            {
                result.Errors.Add("Product not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<ProductQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            products.Name = entity.Name;
            products.Code = entity.Code;
            products.Description = entity.Description;
            products.Price = entity.Price;
            products.Quantity = entity.Quantity;
            products.ImageId = Guid.Parse(entity.ImageId);
            products.CategoryId = Guid.Parse(entity.CategoryId);

            _unitOfWork.GetRepository<Product>().Update(products);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<ProductView>> GetAll()
        {
            var product = await _unitOfWork.GetRepository<Product>().AsQueryable()
                      .Include(x => x.Images)
                      
                      .Select(x => new ProductView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Description = x.Description,
                          Price = x.Price,
                          Quantity = x.Quantity,
                          ImageId = x.ImageId.ToString(),
                          pathImage = x.Images.FilePath,
                          CategoryId = x.CategoryId.ToString(),

                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<ProductView>();
            response.Items = product;
            return response;
        }

        public async Task<DataResult<ProductView>> GetDrinksAsync()
        {
            var drinks = await _unitOfWork.GetRepository<Product>().AsQueryable()
                         .Include(x => x.Images)
                         .Include(x => x.Categorys)
                         .Where(x => x.Categorys.Code == "Drink")
                         .Select(x => new ProductView()
                         {
                             Name = x.Name,
                             Description = x.Description,
                             Price = x.Price,
                             Quantity = x.Quantity,
                             ImageId = x.ImageId.ToString(),
                             CategoryId = x.CategoryId.ToString(),
                             pathImage = x.Images.FilePath,
                             CodeCategory = x.Categorys.Code,
                             NameCategory = x.Categorys.Name,
                         })
                         .ToListAsync();

            var response = new DataResult<ProductView>();
            response.Items = drinks;
            return response;
        }

        public async Task<DataResult<ProductView>> GetBestSellersAsync()
        {
            var bestSellers = await _unitOfWork.GetRepository<Product>().AsQueryable()
                              .Include(x => x.Images)
                              .Include(x => x.Categorys)
                              .Where(x => x.Categorys.Code == "Food")
                              .Select(x => new ProductView()
                              {
                                  Name = x.Name,
                                  Description = x.Description,
                                  Price = x.Price,
                                  Quantity = x.Quantity,
                                  CategoryId = x.CategoryId.ToString(),
                                  ImageId = x.ImageId.ToString(),
                                  pathImage = x.Images.FilePath,
                                  CodeCategory = x.Categorys.Code,
                              })
                              .ToListAsync();

            var response = new DataResult<ProductView>();
            response.Items = bestSellers;
            return response;
        }
    }
}
