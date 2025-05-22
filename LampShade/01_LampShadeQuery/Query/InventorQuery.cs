using _01_LampShadeQuery.Contracts.Inventory;
using InventoryManagement.Infrastructure.EFCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EfCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _01_LampShadeQuery.Query
{
    public class InventorQuery : IInventorQuery
    {
        private readonly InventoryContext _inventoryContext;
        private readonly ShopContext _shopContext;

        public InventorQuery(InventoryContext inventoryContext, ShopContext shopContext)
        {
            _inventoryContext = inventoryContext;
            _shopContext = shopContext;
        }

        public StockStatus CheckStock(IsInStock command)
        {
            var inventory = _inventoryContext.Inventory.AsNoTracking().FirstOrDefault(x => x.ProductId == command.ProductId);
            var product = _shopContext.Products.AsNoTracking().Select(x => new {x.Id, x.Name})
                .FirstOrDefault(x => x.Id == command.ProductId);
            if (inventory == null || inventory.CalculateCurrentCount() < command.Count)
            {
                
                return new StockStatus
                {
                    IsStock = false,
                    ProductName = product?.Name
                };
            }

            return new StockStatus
            {
                IsStock = true,
                ProductName = product?.Name
            };
        }
    }
}
