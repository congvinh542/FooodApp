using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.OrderDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBuy_Api.Service.Validator.ModelValidators
{
    public class OrderDetailsValidator : IValidator<OrderDetailQuery>
    {
        private readonly IOrderDetailsService _orderDetailsService;
        public OrderDetailsValidator(IOrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }

        public async Task<List<ValidationResult>> ValidateAsync(OrderDetailQuery entity, string? userName = null, bool? isUpdate = false)
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
                var orderDetail = await _orderDetailsService.GetByCodeAsync(entity.Code);
                if (orderDetail.Entity != null)
                {
                    return new List<ValidationResult>(){
                    new ValidationResult("OrderDetail is exist", new[] { nameof(entity.Code) })
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