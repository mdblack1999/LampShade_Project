using System.Collections.Generic;
using _01_LampShadeQuery.Contracts.Inventory;
using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Presentation.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventorQuery _inventoryQuery;
        private readonly IInventoryApplication _inventoryApplication;

        public InventoryController(IInventorQuery inventoryQuery, IInventoryApplication inventoryApplication)
        {
            _inventoryQuery = inventoryQuery;
            _inventoryApplication = inventoryApplication;
        }

        [HttpGet("{id}")]
        public List<InventoryOperationViewModel> GetOperationsBy(long id)
        {
            return _inventoryApplication.GetOperationsLog(id);
        }

        [HttpPost]
        public StockStatus CheckStock(IsInStock command)
        {
            return _inventoryQuery.CheckStock(command);
        }
    }
}