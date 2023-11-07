using ClickBuy_Api.Database.Data;
using ClickBuy_Api.DTOs.Models;
using ClickBuy_Api.DTOs.Queries;
using ClickBuy_Api.Service.MailServices;
using ClickBuy_Api.Service.Services;
using ClickBuy_Api.Service.Services.Categorys;
using ClickBuy_Api.Service.Services.DashboardServices;
using ClickBuy_Api.Service.Services.Genders;
using ClickBuy_Api.Service.Services.Huyen;
using ClickBuy_Api.Service.Services.Images;
using ClickBuy_Api.Service.Services.OrderDetails;
using ClickBuy_Api.Service.Services.Orders;
using ClickBuy_Api.Service.Services.PermissionServices;
using ClickBuy_Api.Service.Services.Products;
using ClickBuy_Api.Service.Services.RoleServices;
using ClickBuy_Api.Service.Services.Tinhs;
using ClickBuy_Api.Service.Services.TokenServices;
using ClickBuy_Api.Service.Services.UserServices;
using ClickBuy_Api.Service.Services.Xas;
using ClickBuy_Api.Service.Validator;
using ClickBuy_Api.Service.Validator.ModelValidators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClickBuy_Api.Service.Extensions
{
    public static class ClickBuyExtensionService
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region  Entity DI
            // Default DI
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            // DI Authen
            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            // Custom DI

            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IMailService), typeof(MailService));
            services.AddTransient(typeof(IPermissionService), typeof(PermissionService));
            services.AddTransient(typeof(IRoleService), typeof(RoleService));
            services.AddTransient(typeof(IDashboardService), typeof(DashboardService));

            services.AddTransient(typeof(ICategoryService), typeof(CategoryService));
            services.AddTransient(typeof(IGenderService), typeof(GenderService));
            services.AddTransient(typeof(IHuyenService), typeof(HuyenService));
            services.AddTransient(typeof(IimageService), typeof(ImageService));
            services.AddTransient(typeof(ITinhService), typeof(TinhService));
            services.AddTransient(typeof(IXaService), typeof(XaService));
            services.AddTransient(typeof(IOrderService), typeof(OrderService));
            services.AddTransient(typeof(IOrderDetailsService), typeof(OrderDetailsService));
            services.AddTransient(typeof(IProductService), typeof(ProductService));



            #endregion

            #region Microservice DI
            services.AddControllersWithViews();
            #endregion

            #region Validate Extension DI

            services.AddTransient(typeof(IValidator<RegisterQuery>), typeof(RegisterValidator));
            services.AddTransient(typeof(IValidator<LoginQuery>), typeof(LoginValidator));
            services.AddTransient(typeof(IValidator<PermissionQuery>), typeof(PermissionValidator));
            services.AddTransient(typeof(IValidator<RoleQuery>), typeof(RoleValidator));
            services.AddTransient(typeof(IValidator<CategoryQuery>), typeof(CategoryValidator));
            services.AddTransient(typeof(IValidator<GenderQuery>), typeof(GenderValidator));
            services.AddTransient(typeof(IValidator<HuyenQuery>), typeof(HuyenValidator));
            services.AddTransient(typeof(IValidator<ImageQuery>), typeof(ImageValidator));
            services.AddTransient(typeof(IValidator<TinhQuery>), typeof(TinhValidator));
            services.AddTransient(typeof(IValidator<XaQuery>), typeof(XaValidator));
            services.AddTransient(typeof(IValidator<OrderQuery>), typeof(OrderValidator));
            services.AddTransient(typeof(IValidator<OrderDetailQuery>), typeof(OrderDetailsValidator));
            services.AddTransient(typeof(IValidator<ProductQuery>), typeof(ProductValidator));


            #endregion

            #region Extension DI
            // Add ConnectionString
            services.AddDbContext<ClickBuyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
            ));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            #endregion
            return services;
        }
    }
}
