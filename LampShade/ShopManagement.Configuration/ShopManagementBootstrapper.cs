using _0_Framework.Infrastructure;
using _01_LampshadeQuery.Contracts;
using _01_LampshadeQuery.Query;
using _01_LampShadeQuery.Contracts.Product;
using _01_LampShadeQuery.Contracts.ProductCategory;
using _01_LampShadeQuery.Contracts.Slide;
using _01_LampShadeQuery.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Configuration.Permissions;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.Services;
using ShopManagement.Domain.SlideAgg;
using ShopManagement.Infrastructure.EfCore;
using ShopManagement.Infrastructure.EfCore.Repository;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagement.Infrastructure.InventoryAcl;

namespace ShopManagement.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            //Register Product-Category
            services.AddScoped<IProductCategoryApplication , ProductCategoryApplication>();
            services.AddScoped<IProductCategoryRepository , ProductCategoryRepository>();

            //Register Product
            services.AddScoped<IProductApplication , ProductApplication>();
            services.AddScoped<IProductRepository , ProductRepository>();

            //Register Product-Picture
            services.AddScoped<IProductPictureApplication , ProductPictureApplication>();
            services.AddScoped<IProductPictureRepository , ProductPictureRepository>();

            //Register Slide
            services.AddScoped<ISlideApplication , SlideApplication>();
            services.AddScoped<ISlideRepository , SlideRepository>();

            //Register Order
            services.AddScoped<IOrderRepository , OrderRepository>();
            services.AddScoped<IOrderApplication , OrderApplication>();

            //Register Cart Service
            services.AddSingleton<ICartService , CartService>();

            //Query For Slider UI
            services.AddScoped<ISlideQuery , SlideQuery>();

            //Query For ProductCategory UI
            services.AddScoped<IProductCategoryQuery , ProductCategoryQuery>();

            //Query For Latest Arrivals UI
            services.AddScoped<IProductQuery , ProductQuery>();

            //Register Permissions
            services.AddTransient<IPermissionExposer , ShopPermissionExposer>();

            //Register Cart Calculator
            services.AddTransient<ICartCalculatorService , CartCalculatorService>();

            //Register Acl(Inventory)
            services.AddTransient<IShopInventoryAcl , ShopInventoryAcl>();


            //services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));
            services.AddDbContextPool<ShopContext>(options =>
                options.UseSqlServer(connectionString , sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));
        }
    }
}
