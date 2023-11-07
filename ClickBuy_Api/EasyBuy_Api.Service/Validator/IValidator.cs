using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Validator
{
    public interface IValidator<T>
    {
        Task<List<ValidationResult>> ValidateAsync(T entity, string? userName = null, bool? isUpdate = false);
    }
}