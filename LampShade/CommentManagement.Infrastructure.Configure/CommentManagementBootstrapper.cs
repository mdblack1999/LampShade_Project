using _0_Framework.Infrastructure;
using CommentManagement.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.Configure.Permission;
using CommentManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommentManagement.Infrastructure.Configure
{
    public class CommentManagementBootstrapper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            //Register Comment
            services.AddScoped<ICommentRepository , CommentRepository>();
            services.AddScoped<ICommentApplication , CommentApplication>();

            //Register Permission
            services.AddTransient<IPermissionExposer , CommentPermissionExposer>();

            services.AddDbContextPool<CommentContext>(options =>
                options.UseSqlServer(connectionString , sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));
        }
    }
}
