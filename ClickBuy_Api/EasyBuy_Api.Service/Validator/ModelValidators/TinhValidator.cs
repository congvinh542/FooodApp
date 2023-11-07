using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Huyen;
using ClickBuy_Api.Service.Services.Tinhs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class TinhValidator : IValidator<TinhQuery>
    {
        private readonly ITinhService _tinhService;
        public TinhValidator(ITinhService tinhService)
        {
            _tinhService = tinhService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(TinhQuery entity, string? userName = null, bool? isUpdate = false)
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
                var tinh = await _tinhService.GetByCodeAsync(entity.Code);
                if (tinh.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Tinh is exist", new[] { nameof(entity.Code) })
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
