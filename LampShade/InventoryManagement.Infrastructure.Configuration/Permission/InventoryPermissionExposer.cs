using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permission
{
    public class InventoryPermissionExposer :IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "Inventory", new List<PermissionDto>
                    {
                        new PermissionDto(InventoryPermissions.ListInventory,"ListInventory"),
                        new PermissionDto(InventoryPermissions.HistoryLog,"HistoryInventory"),
                        new PermissionDto(InventoryPermissions.Increase,"IncreaseInventory"),
                        new PermissionDto(InventoryPermissions.Decrease,"DecreaseInventory"),
                        new PermissionDto(InventoryPermissions.CreateInventory,"CreateInventory"),
                        new PermissionDto(InventoryPermissions.EditInventory,"EditInventory"),
                    }
                }
            };
        }
    }
}
