using ClickBuy_Api.Database.Entities;
using ClickBuy_Api.Database.Entities.Products;
using ClickBuy_Api.DTOs.Fillters;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.DTOs.Views;
using ClickBuy_Api.SDK.Extensions;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Helpers;
using ClickBuy_Api.Service.Services.BaseService;
using ClickBuy_Api.Service.Services.Products;
using ClickBuy_Api.Service.Services.UserServices;
using ClickBuy_Api.Service.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace ClickBuy_Api.Service.Services.Orders
{
    public class OrderService : ServiceBase<IUnitOfWork>, IOrderService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IUserService userService, IProductService productService, IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DataResult<bool>> CreateAsync(OrderQuery entity)
        {
            var result = new DataResult<bool>();

            var order = new Order
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                UserId = new Guid("832E7728-9B34-4E9C-8E2E-08DBDE912307"), // Gán ID của người dùng đặt hàng vào đơn hàng
                OrderDate = DateTime.Now, // Ngày đặt hàng
            };

            // Thêm chi tiết đơn hàng
   
            

            await _unitOfWork.GetRepository<Order>().Add(order);
            result.Entity = await _unitOfWork.SaveChangesAsync() > 0;

            if (result.Entity == false)
            {
                result.Errors.Add("Error while saving.");
                return result;
            }

            return result;
        }

        public async Task<DataResult<int>> DeleteAsync(string id)
        {
            var result = new DataResult<int>();
            var Order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(Guid.Parse(id));
            if (Order == null)
            {
                result.Errors.Add("Permission not found");
                return result;
            }
            _unitOfWork.GetRepository<Order>().Delete(Order, false);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<OrderView>> GetByCodeAsync(string code)
        {
            var result = new DataResult<OrderView>();
            var order = await _unitOfWork.GetRepository<Order>().AsQueryable().FirstOrDefaultAsync(x => x.Code == code);
            if (order == null)
            {
                result.Errors.Add("Order not found");
                return result;
            }
            result.Entity = new OrderView
            {
                Id = order.Id,
                Name = order.Name,
                Code = order.Code,
                Description = order.Description,
            };
            return result;
        }

        public async Task<DataResult<OrderView>> GetByIdAsync(string id)
        {
            var result = new DataResult<OrderView>();
            var Order = await _unitOfWork.GetRepository<Order>()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (Order == null)
            {
                result.Errors.Add("Order not found");
                return result;
            }
            result.Entity = new OrderView
            {
                Id = Order.Id,
                Code = Order.Code,
                Name = Order.Name,
                Description = Order.Description,

                CreatedBy = Order.CreatedBy,
                CreatedAt = Order.CreatedAt,
                UpdatedAt = Order.UpdatedAt,
                IsActive = Order.IsActive,
            };
            return result;
        }

        public async Task<DataResult<OrderView>> GetPageList(BaseFilter<OrderFilter> query)
        {
            var Orders = await _unitOfWork.GetRepository<Order>().AsQueryable()
                     .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                     .Take(query.PageSize.Value)
                     .Select(x => new OrderView()
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

            var response = new DataResult<OrderView>();
            response.TotalRecords = await _unitOfWork.GetRepository<Order>().AsQueryable().CountAsync();
            response.Items = Orders;
            return response;
        }

        public async Task<DataResult<int>> UpdateAsync(OrderQuery entity, string id)
        {
            var result = new DataResult<int>();
            var order = await _unitOfWork.GetRepository<Order>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (order == null)
            {
                result.Errors.Add("Order not found");
                return result;
            }
            // Validate
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IValidator<OrderQuery>>();
            List<ValidationResult> resultValidator = new List<ValidationResult>();
            if (validator != null)
                resultValidator = await validator.ValidateAsync(entity);
            if (resultValidator.HasError())
            {
                result.Errors.AddRange(resultValidator.JoinError());
                return result;
            }
            order.Name = entity.Name;
            order.Code = entity.Code;
            order.Description = entity.Description;

            _unitOfWork.GetRepository<Order>().Update(order);
            result.Entity = await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<DataResult<OrderView>> GetAll()
        {
            var order = await _unitOfWork.GetRepository<Order>().AsQueryable()
                      .Select(x => new OrderView()
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
            var response = new DataResult<OrderView>();
            response.Items = order;
            return response;
        }
    }
}