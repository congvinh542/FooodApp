using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Huyen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class HuyenValidator : IValidator<HuyenQuery>
    {
        private readonly IHuyenService _huyenService;
        public HuyenValidator(IHuyenService huyenService)
        {
            _huyenService = huyenService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(HuyenQuery entity, string? userName = null, bool? isUpdate = false)
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
                var huyen = await _huyenService.GetByCodeAsync(entity.Code);
                if (huyen.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Huyen is exist", new[] { nameof(entity.Code) })
                };
                }
            }

            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.Name), entity.Name),
                ValidatorCustom.IsRequired(nameof(entity.TinhId), entity.TinhId),
            };
            return result;
        }
    }
}

