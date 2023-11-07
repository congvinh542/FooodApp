using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Xas;
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class XaValidator : IValidator<XaQuery>
    {
        private readonly IXaService _xaService;
        public XaValidator(IXaService xaService)
        {
            _xaService = xaService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(XaQuery entity, string? userName = null, bool? isUpdate = false)
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
                var xa = await _xaService.GetByCodeAsync(entity.Code);
                if (xa.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Xa is exist", new[] { nameof(entity.Code) })
                };
                }
            }

            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.Name), entity.Name),
                ValidatorCustom.IsRequired(nameof(entity.HuyenId), entity.HuyenId),
            };
            return result;
        }
    }
}