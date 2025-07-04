using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using InventoryManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages.Inventory
{
    [Authorize(Roles = Roles.Administrator)]
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public InventorySearchModel SearchModel;
        public List<InventoryViewModel> Inventory;
        public SelectList Products;

        [BindProperty]
        public string TableJson { get; set; }

        private readonly IProductApplication _productApplication;
        private readonly IInventoryApplication _inventoryApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(IProductApplication productApplication , IInventoryApplication inventoryApplication, IReportExporter reportExporter)
        {
            _productApplication = productApplication;
            _inventoryApplication = inventoryApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(InventoryPermissions.ListInventory)]
        public void OnGet(InventorySearchModel searchModel)
        {
            Products = new SelectList(_productApplication.GetProducts() , "Id" , "Name");
            Inventory = _inventoryApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateInventory
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create" , command);
        }

        [NeedsPermission(InventoryPermissions.CreateInventory)]
        public JsonResult OnPostCreate(CreateInventory command)
        {
            var result = _inventoryApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var inventory = _inventoryApplication.GetDetails(id);
            inventory.Products = _productApplication.GetProducts();
            return Partial("Edit" , inventory);
        }

        [NeedsPermission(InventoryPermissions.EditInventory)]
        public JsonResult OnPostEdit(EditInventory command)
        {
            var result = _inventoryApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetIncrease(long id , string name)
        {
            var command = new IncreaseInventory()
            {
                InventoryId = id ,
                Product = name
            };
            return Partial("Increase" , command);
        }

        [NeedsPermission(InventoryPermissions.Increase)]
        public JsonResult OnPostIncrease(IncreaseInventory command)
        {
            var result = _inventoryApplication.Increase(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetReduce(long id , string name)
        {
            var command = new ReduceInventory()
            {
                InventoryId = id ,
                Product = name
            };
            return Partial("Reduce" , command);
        }

        [NeedsPermission(InventoryPermissions.Reduce)]
        public JsonResult OnPostReduce(ReduceInventory command)
        {
            var result = _inventoryApplication.Reduce(command);
            return new JsonResult(result);
        }
        [NeedsPermission(InventoryPermissions.OperationLog)]
        public IActionResult OnGetLog(long id)
        {
            var log = _inventoryApplication.GetOperationsLog(id);
            return Partial("OperationLog" , log);
        }

        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<InventoryViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "Inventory.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "InventoryReport.xlsx");
        }
    }
}
