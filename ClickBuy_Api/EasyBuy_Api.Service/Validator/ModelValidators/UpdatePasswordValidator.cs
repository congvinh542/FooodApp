using System.ComponentModel.DataAnnotations;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class UpdatePasswordValidator : IValidator<UpdatePasswordQuery>
    {
        public async Task<List<ValidationResult>> ValidateAsync(UpdatePasswordQuery entity, string? userName = null, bool? isUpdate = false)
        {
            if (entity == null)
            {
                return new List<ValidationResult>(){
                    new ValidationResult("Entity is null", new[] { nameof(entity) })
                };
            }
            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.OldPassword), entity.OldPassword),
                ValidatorCustom.Password(nameof(entity.OldPassword), entity.OldPassword),
                
                ValidatorCustom.IsRequired(nameof(entity.NewPassword), entity.NewPassword),
                ValidatorCustom.Password(nameof(entity.NewPassword), entity.NewPassword),
                
                ValidatorCustom.IsRequired(nameof(entity.ConfirmPassword), entity.ConfirmPassword),
                ValidatorCustom.Password(nameof(entity.ConfirmPassword), entity.ConfirmPassword),
                ValidatorCustom.ConfirmPassword(nameof(entity.ConfirmPassword), entity.ConfirmPassword, entity.NewPassword),
            };  
            return await Task.FromResult(result);
        }
    }
}