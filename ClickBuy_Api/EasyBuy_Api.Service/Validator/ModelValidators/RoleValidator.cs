using System.ComponentModel.DataAnnotations;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.RoleServices;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class RoleValidator : IValidator<RoleQuery>
    {
        private readonly IRoleService _roleService;
        public RoleValidator(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<List<ValidationResult>> ValidateAsync(RoleQuery entity, string? userName = null, bool? isUpdate = false)
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
                var role = await _roleService.GetByIdAsync(entity.Name);
                if (role != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Role is exist", new[] { nameof(entity.Name) })
                };
                }
            }

            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.Name), entity.Name),
            };
            return await Task.FromResult(result);
        }
    }
}