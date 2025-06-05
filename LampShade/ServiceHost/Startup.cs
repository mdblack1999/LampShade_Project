using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using _0_Framework.Application.Sms;
using _0_Framework.Application.ZarinPal;
using _0_Framework.Infrastructure;
using _01_LampShadeQuery.Contracts.ReportingManagement.Export;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using AccountManagement.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configure;
using DiscountManagement.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using InventoryManagement.Presentation.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceHost.Controller;
using ShopManagement.Configuration;

namespace ServiceHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("LampShadeDb");

            ShopManagementBootstrapper.Configure(services , connectionString);
            DiscountManagementBootstrapper.Configure(services , connectionString);
            InventoryManagementBootstrapper.Configure(services , connectionString);
            BlogManagementBootstrapper.Configure(services , connectionString);
            CommentManagementBootstrapper.Configure(services , connectionString);
            AccountManagementBootstrapper.Configure(services , connectionString);

            services.AddHttpContextAccessor();
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin , UnicodeRanges.Arabic));
            services.AddSingleton<IPasswordHasher , PasswordHasher>();
            services.AddTransient<IFileUploader , FileUploader>();
            services.AddTransient<IAuthHelper , AuthHelper>();
            services.AddTransient<IZarinPalFactory , ZarinPalFactory>();
            services.AddTransient<ISmsService , SmsService>();
            services.AddTransient<IEmailService , EmailService>();
            services.AddTransient<IReportExporter, ReportExporter>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme ,
                    op =>
                {
                    op.LoginPath = new PathString("/Account");
                    op.LogoutPath = new PathString("/Account");
                    op.AccessDeniedPath = new PathString("/AccessDenied");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator , Roles.ContentAdmin
                        ,Roles.DiscountAdmin,Roles.ProductAdmin,Roles.StoreKeeper}));

                options.AddPolicy("Shop" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator , Roles.ProductAdmin }));

                options.AddPolicy("Discount" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator , Roles.DiscountAdmin }));

                options.AddPolicy("Account" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator }));

                options.AddPolicy("Inventory" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator , Roles.StoreKeeper }));

                options.AddPolicy("Comment" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator , Roles.ContentAdmin }));

                options.AddPolicy("Blogs" ,
                    builder => builder.RequireRole(new List<string> { Roles.Administrator , Roles.ContentAdmin }));
            });

            
            services.AddControllers();


            services.AddCors(option => option.AddPolicy("MyPolicy" , builder =>
                builder.WithOrigins("https://localhost:5002")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                ));

            services.AddRazorPages()
                .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/" , "AdminArea");
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/Shop" , "Shop");
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/Discounts" , "Discount");
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/Accounts" , "Account");
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/Inventory" , "Inventory");
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/Comments" , "Comment");
                    options.Conventions.AuthorizeAreaFolder("Administration" , "/Blog" , "Blogs");
                }).AddApplicationPart(typeof(ProductController).Assembly)
                .AddApplicationPart(typeof(InventoryController).Assembly)
                .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("MyPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

    }
}
