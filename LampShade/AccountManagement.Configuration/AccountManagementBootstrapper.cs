using _0_Framework.Infrastructure;
using AccountManagement.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Configuration.Permission;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Infrastructure.EFCore;
using AccountManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Configuration
{
    public class AccountManagementBootstrapper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            //Register Accounts
            services.AddTransient<IAccountApplication , AccountApplication>();
            services.AddTransient<IAccountRepository , AccountRepository>();

            //Register Role
            services.AddTransient<IRoleApplication , RoleApplication>();
            services.AddTransient<IRoleRepository , RoleRepository>();

            //Register Permission
            services.AddTransient<IPermissionExposer, AccountPermissionExposer>();


            services.AddDbContextPool<AccountContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
