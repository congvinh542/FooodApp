using System.ComponentModel.DataAnnotations;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class PermissionValidator : IValidator<PermissionQuery>
    {
        private readonly Services.PermissionServices.IPermissionService _permissonService;
        public PermissionValidator(Services.PermissionServices.IPermissionService permissonService)
        {
            _permissonService = permissonService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(PermissionQuery entity, string? userName = null, bool? isUpdate = false)
        {
            if (entity == null)
            {
                return new List<ValidationResult>(){
                    new ValidationResult("Entity is null", new[] { nameof(entity) })
                };
            }
            // check exist
            if (isUpdate != true)
            {
                var permission = await _permissonService.GetByIdAsync(entity.Id.ToString());
                if (permission != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Permission is exist", new[] { nameof(entity.PermissionKey) })
                };
                }
            }

            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.PermissionKey), entity.PermissionKey),
                ValidatorCustom.IsRequired(nameof(entity.PermissionValue), entity.PermissionKey),
            };
            return await Task.FromResult(result);
        }
    }
}