using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;
using System.Collections.Generic;
using System.Linq;
using AccountManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository : RepositoryBase<long , Inventory>, IInventoryRepository
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;
        private readonly AccountContext _accountContext;

        public InventoryRepository(InventoryContext inventoryContext , ShopContext shopContext , AccountContext accountContext) : base(inventoryContext)
        {
            _inventoryContext = inventoryContext;
            _shopContext = shopContext;
            _accountContext = accountContext;
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
            var account = _accountContext.Accounts.AsNoTracking().Select(x => new { x.Id , x.FullName }).ToList();
            var inventory = _inventoryContext.Inventory.AsNoTracking().FirstOrDefault(x => x.Id == inventoryId);
            if (inventory == null)
                return new List<InventoryOperationViewModel>
                {
                   new InventoryOperationViewModel
                   {
                       Count = 0,
                       Description = "متاسفانه با مشخصات فعلی هیچ عملیاتی یافت نشد"
                   }
                };
            var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id ,
                Count = x.Count ,
                CurrentCount = x.CurrentCount ,
                Description = x.Description ,
                Operation = x.Operation ,
                OperationDate = x.OperationDate.ToFarsi() ,
                OperatorId = x.OperatorId ,
                OrderId = x.OrderId
            }).OrderByDescending(x => x.Id).ToList();

            foreach (var operation in operations)
            {
                operation.Operator = account.FirstOrDefault(x => x.Id == operation.OperatorId)?.FullName;
            }

            return operations;
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

            if (searchModel.InStock)
            {
                query = query.Where(x => !x.InStock);
            }
            else
            {
                query = query.Where(x => x.InStock);
            }

            if (searchModel.ProductId > 0)
            {
                query = query.Where(x => x.ProductId == searchModel.ProductId);
            }

            var inventory = query.OrderByDescending(x => x.Id).AsNoTracking().ToList();

            inventory.ForEach(item =>
              item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name);

            return inventory;
        }
    }
}
