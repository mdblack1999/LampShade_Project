﻿using _0_Framework.Infrastructure;
using _01_LampShadeQuery.Contracts.Inventory;
using _01_LampShadeQuery.Query;
using InventoryManagement.Application;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.Configuration.Permission;
using InventoryManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infrastructure.Configuration
{
    public class InventoryManagementBootstrapper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            //Register Inventory
            services.AddScoped<IInventoryRepository , InventoryRepository>();
            services.AddScoped<IInventoryApplication , InventoryApplication>();

            //Register Permission
            services.AddTransient<IPermissionExposer , InventoryPermissionExposer>();

            //Register Query
            services.AddTransient<IInventorQuery , InventorQuery>();


            services.AddDbContextPool<InventoryContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
