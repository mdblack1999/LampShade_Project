using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository : RepositoryBase<long , Inventory>, IInventoryRepository
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;

        public InventoryRepository(InventoryContext inventoryContext , ShopContext shopContext) : base(inventoryContext)
        {
            _inventoryContext = inventoryContext;
            _shopContext = shopContext;
        }

        public Inventory GetBy(long productId)
        {
            return _inventoryContext.Inventory.FirstOrDefault(x => x.ProductId == productId);
        }

        public EditInventory GetDetails(long id)
        {
            return _inventoryContext.Inventory.AsNoTracking().Select(x => new EditInventory
            {
                Id = x.Id ,
                ProductId = x.ProductId ,
                UnitPrice = x.UnitPrice 
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<InventoryOperationViewModel> GetOperationsLog(long inventoryId)
        {
            var inventory = _inventoryContext.Inventory.AsNoTracking().FirstOrDefault(x => x.Id == inventoryId);
            return inventory.Operations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id ,
                Count = x.Count ,
                CurrentCount = x.CurrentCount ,
                Description = x.Description ,
                Operation = x.Operation ,
                OperationDate = x.OperationDate.ToFarsi() ,
                Operator = "مدیر سیستم" ,
                OperatorId = x.OperatorId ,
                OrderId = x.OrderId
            }).OrderByDescending(x=>x.Id).ToList();
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _shopContext.Products.AsNoTracking().Select(x => new { x.Id , x.Name }).AsNoTracking().ToList();
            var query = _inventoryContext.Inventory.Select(x => new InventoryViewModel
            {
                Id = x.Id ,
                ProductId = x.ProductId ,
                UnitPrice = x.UnitPrice ,
                InStock = x.InStock ,
                CurrentCount = x.CalculateCurrentCount() ,
                CreationDate = x.CreationDate.ToFarsi()
            });
            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (searchModel.InStock)
                query = query.Where(x => !x.InStock);

            List<InventoryViewModel> inventory = query.OrderByDescending(x => x.Id).ToList();

            inventory.ForEach(item =>
              item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name);

            return inventory;
        }
    }
}
