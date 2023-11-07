using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Categorys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class CategoryValidator : IValidator<CategoryQuery>
    {
        private readonly ICategoryService _categoryService;
        public CategoryValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(CategoryQuery entity, string? userName = null, bool? isUpdate = false)
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
                var category = await _categoryService.GetByCodeAsync(entity.Code);
                if (category.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Category is exist", new[] { nameof(entity.Code) })
                };
                }
            }

            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.Name), entity.Name),
            };
            return result;
        }
    }
}