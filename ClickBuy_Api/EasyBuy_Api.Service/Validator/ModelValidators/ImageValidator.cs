using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Services.Images;
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class ImageValidator : IValidator<ImageQuery>
    {
        private readonly IimageService _ImageService;
        public ImageValidator(IimageService ImageService)
        {
            _ImageService = ImageService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(ImageQuery entity, string? userName = null, bool? isUpdate = false)
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
                var subject = await _ImageService.GetByCodeAsync(entity.Code);
                if (subject.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Subject is exist", new[] { nameof(entity.Code) })
                };
                }
            }

            var result = new List<ValidationResult>()
            {

            };
            return result;
        }
    }
}
