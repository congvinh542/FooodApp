using ClickBuy_Api.Database.Common;
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

            try
            {
                var order = new Order
                {
                    Name = entity.Name,
                    Code = HelperCommon.GenerateCode(8, "#"),
                    TotalAmount = decimal.Parse(entity.TotalAmount),
                    PathImage = entity.PathImage,
                    Quantity = entity.Quantity,
                    UserId = new Guid("A21D05FD-801A-4AA6-526D-08DBEB372DB8"), // Gán ID của người dùng đặt hàng vào đơn hàng
                    OrderDate = DateTime.Now, // Ngày đặt hàng
                };

                await _unitOfWork.GetRepository<Order>().Add(order);
                result.Entity = await _unitOfWork.SaveChangesAsync() > 0;

                if (!result.Entity)
                {
                    result.Errors.Add("Error while saving.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                result.Errors.Add($"Error during data insertion: {ex.Message}");
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
                         UserId = x.UserId.ToString(),
                         Description = x.Description,
                         UserName = _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.System.User>().AsQueryable()
                                      .Where(name => name.Id == x.UserId)
                                      .Select(name => name.FirstName)
                                      .FirstOrDefault(),
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

        public async Task<DataResult<List<OrderView>>> GetOrderByUserId(string id)
        {
            var result = new DataResult<List<OrderView>>();
            var orders = await _unitOfWork.GetRepository<Order>()
                .AsQueryable()
                .Include(x => x.User)
                .Where(x => x.UserId == Guid.Parse(id))
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                result.Errors.Add("No orders found for the given category");
                return result;
            }

            result.Entity = orders.Select(order => new OrderView
            {
                Id = order.Id,
                Code = order.Code,
                Name = order.Name,
                UserId = order.UserId.ToString(),
                UserName = _unitOfWork.GetRepository<ClickBuy_Api.Database.Entities.System.User>().AsQueryable()
                                      .Where(name => name.Id == order.UserId)
                                      .Select(name => name.FirstName)
                                      .FirstOrDefault(),
                Description = order.Description,
                Quantity = order.Quantity,
                PathImage = order.PathImage,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                CreatedBy = order.CreatedBy,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                IsActive = order.IsActive,
            }).ToList();

            return result;
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
                          UserId = x.UserId.ToString(),
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