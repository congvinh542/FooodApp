using System.ComponentModel.DataAnnotations;
using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using Microsoft.AspNetCore.Identity;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class ResetPasswordValidator : IValidator<UpdatePasswordQuery>
    {
        private readonly UserManager<User> _userManager;
        public ResetPasswordValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
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
            // check tokem is valid
            var user = await _userManager.FindByEmailAsync(entity.Email);
            if (user == null)
            {
                result.Add(new ValidationResult("Email is not exist", new[] { nameof(entity.Email) }));
            }
            else
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, entity.OldPassword);
                if (!checkPassword)
                {
                    result.Add(new ValidationResult("Old password is not correct", new[] { nameof(entity.OldPassword) }));
                }
            }
            return await Task.FromResult(result);
        }
    }
}