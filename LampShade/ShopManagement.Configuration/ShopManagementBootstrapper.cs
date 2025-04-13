using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EfCore;
using ShopManagement.Infrastructure.EfCore.Repository;

namespace ShopManagement.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection services,string ConnectionString)
        {
            //Register Product-Category
            services.AddTransient<IProductCategoryApplication , ProductCategoryApplication>();
            services.AddTransient<IProductCategoryRepository , ProductCategoryRepository>();

            //Register Product
            services.AddTransient<IProductApplication , ProductApplication>();
            services.AddTransient<IProductRepository , ProductRepository>();

            //Register Product-Picture
            services.AddTransient<IProductPictureApplication , ProductPictureApplication>();
            services.AddTransient<IProductPictureRepository , ProductPictureRepository>();

            services.AddDbContext<ShopContext>(x => x.UseSqlServer(ConnectionString));
        }
    }
}
