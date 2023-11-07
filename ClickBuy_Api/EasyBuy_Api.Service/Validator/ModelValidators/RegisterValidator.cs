using System.ComponentModel.DataAnnotations;
using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using Microsoft.AspNetCore.Identity;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class RegisterValidator : IValidator<RegisterQuery>
    {
        private readonly UserManager<User> _userManager;
        public RegisterValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<ValidationResult>> ValidateAsync(RegisterQuery entity, string? userName = null, bool? isUpdate = false)
        {
            if (entity == null)
            {
                return new List<ValidationResult>(){
                    new ValidationResult("Entity is null", new[] { nameof(entity) })
                };
            }
            var result = new List<ValidationResult>(){
                ValidatorCustom.IsRequired(nameof(entity.FirstName), entity.FirstName),
                ValidatorCustom.MinLength(nameof(entity.FirstName), entity.FirstName, 2),
                ValidatorCustom.MaxLength(nameof(entity.FirstName), entity.FirstName, 10),
                ValidatorCustom.IsRequired(nameof(entity.LastName), entity.LastName),
                ValidatorCustom.MinLength(nameof(entity.LastName), entity.LastName, 2),
                ValidatorCustom.MaxLength(nameof(entity.LastName), entity.LastName, 10),
                // Email
                ValidatorCustom.IsRequired(nameof(entity.Email), entity.Email),
                ValidatorCustom.Email(nameof(entity.Email), entity.Email),
                // Password
                ValidatorCustom.IsRequired(nameof(entity.Password), entity.Password),
                ValidatorCustom.Password(nameof(entity.Password), entity.Password),
                // Confirm Password
                ValidatorCustom.IsRequired(nameof(entity.ComfirmPassword), entity.ComfirmPassword),
            };

            // check email exist
            var user = await _userManager.FindByEmailAsync(entity.Email);
            if (user != null)
            {
                result.Add(new ValidationResult("Email already exists", new[] { nameof(entity.Email) }));
            }
            return await Task.FromResult(result);
        }

    }
}