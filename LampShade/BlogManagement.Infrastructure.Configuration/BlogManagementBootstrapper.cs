using BlogManagement.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore;
using BlogManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogManagement.Infrastructure.Configuration
{
    public class BlogManagementBootstrapper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            //Register Article Category
            services.AddScoped<IArticleCategoryApplication , ArticleCategoryApplication>();
            services.AddScoped<IArticleCategoryRepository , ArticleCategoryRepository>();

            //Register Article
            services.AddScoped<IArticleApplication , ArticleApplication>();
            services.AddScoped<IArticleRepository , ArticleRepository>();

            services.AddDbContext<BlogContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
