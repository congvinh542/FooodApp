using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Genders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    partial class GenderValidator : IValidator<GenderQuery>
    {
        private readonly IGenderService _genderService;
        public GenderValidator(IGenderService genderService)
        {
            _genderService = genderService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(GenderQuery entity, string? userName = null, bool? isUpdate = false)
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
                var subject = await _genderService.GetByCodeAsync(entity.Code);
                if (subject.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Gender is exist", new[] { nameof(entity.Code) })
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
