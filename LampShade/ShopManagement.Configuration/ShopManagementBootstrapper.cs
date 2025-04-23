using _01_LampShadeQuery.Contracts.Product;
using _01_LampShadeQuery.Contracts.ProductCategory;
using _01_LampShadeQuery.Contracts.Slide;
using _01_LampShadeQuery.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.SlideAgg;
using ShopManagement.Infrastructure.EfCore;
using ShopManagement.Infrastructure.EfCore.Repository;

namespace ShopManagement.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection services , string ConnectionString)
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

            //Query For Slider UI
            services.AddScoped<ISlideQuery , SlideQuery>();

            //Query For ProductCategory UI
            services.AddScoped<IProductCategoryQuery , ProductCategoryQuery>();
            //Query For Latest Arrivals UI
            services.AddScoped<IProductQuery , ProductQuery>();



            //services.AddDbContext<ShopContext>(x => x.UseSqlServer(ConnectionString));
            services.AddDbContextPool<ShopContext>(options =>
                options.UseSqlServer(ConnectionString , sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(); // برای هندل خطاهای موقتی دیتابیس
                }));
        }
    }
}
