using DiscountManagement.Application;
using DiscountManagement.Application.Contract.ColleagueDiscount;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountManagement.Configuration
{
    public class DiscountManagementBootstraper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            //Register Customer Discount
            services.AddScoped<ICustomerDiscountApplication , CustomerDiscountApplication>();
            services.AddScoped<ICustomerDsicountRepository , CustomerDiscountRepository>();

            //Register Colleague Discount
            services.AddScoped<IColleagueDiscountApplication , ColleagueDiscountApplication>();
            services.AddScoped<IColleagueDiscountRepository , ColleagueDiscountRepository>();

            //register Context
            services.AddDbContextPool<DiscountContext>(options =>
                options.UseSqlServer(connectionString , sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();   
                }));

        }
    }
}
