using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permission
{
    public class InventoryPermissionExposer : IPermissionExposer
    {
        public Dictionary<string , List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "INVENTORY", new List<PermissionDto>
                    {
                        new PermissionDto(InventoryPermissions.ListInventory,"مشاهده انبار داری"),
                        new PermissionDto(InventoryPermissions.SearchInventory,"جستجو در انبارداری"),
                        new PermissionDto(InventoryPermissions.CreateInventory,"ساخت انبار جدید"),
                        new PermissionDto(InventoryPermissions.EditInventory,"ویرایش انبار ها"),
                        new PermissionDto(InventoryPermissions.OperationLog,"مشاهده گردش ها"),
                        new PermissionDto(InventoryPermissions.Increase,"افزایش موجودی"),
                        new PermissionDto(InventoryPermissions.Reduce,"کاهش موجودی"),
                    }
                }
            };
        }
    }
}
