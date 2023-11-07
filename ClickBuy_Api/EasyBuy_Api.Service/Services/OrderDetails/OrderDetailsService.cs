using ClickBuy_Api.Database.Entities.Products;
using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.SDK.Extensions;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using ClickBuy_Api.Service.Validator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Services.OrderDetails
{
    public class OrderDetailsService : ServiceBase<IUnitOfWork>, IOrderDetailsService
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderDetailsService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataResult<bool>> CreateAsync(OrderDetailQuery entity)
        {
            var result = new DataResult<bool>();
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<OrderDetailQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            var orderDetails = new OrderDetail
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
            };
            await _unitOfWork.GetRepository<OrderDetail>().Add(orderDetails);
            result.Entity = await _unitOfWork.SaveChangesAsync() > 0;
            if (result.Entity == false)
            {
                result.Errors.Add("Error while saving");
                return result;
            }
            return result;
        }

        public async Task<DataResult<int>> DeleteAsync(string id)
        {
            var result = new DataResult<int>();
            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>().GetByIdAsync(Guid.Parse(id));
            if (orderDetails == null)
            {
                result.Errors.Add("OrderDetails not found");
                return result;
            }
            _unitOfWork.GetRepository<OrderDetail>().Delete(orderDetails, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<OrderDetailView>> GetByCodeAsync(string OrderDetailsCode)
        {
            var result = new DataResult<OrderDetailView>();
            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>().AsQueryable().FirstOrDefaultAsync(x => x.Code == OrderDetailsCode);
            if (orderDetails == null)
            {
                result.Errors.Add("OrderDetails not found");
                return result;
            }
            result.Entity = new OrderDetailView
            {
                Id = orderDetails.Id,
                Name = orderDetails.Name,
                Code = orderDetails.Code,
                Description = orderDetails.Description,
            };
            return result;
        }

        public async Task<DataResult<OrderDetailView>> GetByIdAsync(string id)
        {
            var result = new DataResult<OrderDetailView>();
            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (orderDetails == null)
            {
                result.Errors.Add("OrderDetails not found");
                return result;
            }
            result.Entity = new OrderDetailView
            {
                Id = orderDetails.Id,
                Code = orderDetails.Code,
                Name = orderDetails.Name,
                Description = orderDetails.Description,

                CreatedBy = orderDetails.CreatedBy,
                CreatedAt = orderDetails.CreatedAt,
                UpdatedAt = orderDetails.UpdatedAt,
                IsActive = orderDetails.IsActive,
            };
            return result;
        }

        public async Task<DataResult<OrderDetailView>> GetPageList(BaseFilter<OrderDetailsFilter> query)
        {
            var orderDetailss = await _unitOfWork.GetRepository<OrderDetail>().AsQueryable()
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new OrderDetailView()
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Code = x.Code,
                         Description = x.Description,

                         CreatedBy = x.CreatedBy,
                         CreatedAt = x.CreatedAt,
                         UpdatedAt = x.UpdatedAt,
                         IsActive = x.IsActive,
                     })
                     .ApplyFilter(query)
                     .OrderByColums(query.SortColums, true).ToListAsync();

            var response = new DataResult<OrderDetailView>();
            response.TotalRecords = await _unitOfWork.GetRepository<OrderDetail>().AsQueryable().CountAsync();
            response.Items = orderDetailss;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(OrderDetailQuery entity, string id)
        {
            var result = new DataResult<int>();
            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (orderDetails == null)
            {
                result.Errors.Add("OrderDetails not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<OrderDetailQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            orderDetails.Name = entity.Name;
            orderDetails.Code = entity.Code;
            orderDetails.Description = entity.Description;

            _unitOfWork.GetRepository<OrderDetail>().Update(orderDetails);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<OrderDetailView>> GetAll()
        {
            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>().AsQueryable()
                      .Select(x => new OrderDetailView()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Code = x.Code,
                          Description = x.Description,
                          CreatedBy = x.CreatedBy,
                          CreatedAt = x.CreatedAt,
                          UpdatedAt = x.UpdatedAt,
                          IsActive = x.IsActive,
                      }).ToListAsync();
            var response = new DataResult<OrderDetailView>();
            response.Items = orderDetails;
            return response;
        }
    }
}