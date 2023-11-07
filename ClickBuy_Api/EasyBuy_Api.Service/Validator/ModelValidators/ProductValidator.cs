using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Products;
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class ProductValidator : IValidator<ProductQuery>
    {
        private readonly IProductService _productService;
        public ProductValidator(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(ProductQuery entity, string? userName = null, bool? isUpdate = false)
        {


            if (entity == null)
            {
                return new List<ValidationResult>(){
                    new ValidationResult("Entity is null", new[] { nameof(entity) })
                };
            }
            // check exist for add  
            if (isUpdate == false)
            {
                var product = await _productService.GetByCodeAsync(entity.Code);
                if (product.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Product is exist", new[] { nameof(entity.Code) })
                };
                }
            }

            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.Name), entity.Name),
                ValidatorCustom.IsRequired(nameof(entity.ImageId), entity.ImageId),
            };
            return result;
        }
    }
}