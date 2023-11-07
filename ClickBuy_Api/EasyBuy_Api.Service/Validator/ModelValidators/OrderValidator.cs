using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class OrderValidator : IValidator<OrderQuery>
    {
        private readonly IOrderService _orderService;
        public OrderValidator(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(OrderQuery entity, string? userName = null, bool? isUpdate = false)
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
                var order = await _orderService.GetByCodeAsync(entity.Code);
                if (order.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("Order is exist", new[] { nameof(entity.Code) })
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