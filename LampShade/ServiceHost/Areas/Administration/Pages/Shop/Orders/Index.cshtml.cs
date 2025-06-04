// File: ServiceHost/Areas/Administration/Pages/Shop/Orders/Index.cshtml.cs
using System.Collections.Generic;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.Orders
{
    public class IndexModel : PageModel
    {
        public OrderSearchModel SearchModel;
        public SelectList Accounts;
        public List<OrderViewModel> Orders;

        [BindProperty]
        public string TableJson { get; set; }

        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(
            IOrderApplication orderApplication,
            IAccountApplication accountApplication,
            IReportExporter reportExporter)
        {
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(ShopPermissions.ListOrders)]
        public void OnGet(OrderSearchModel searchModel)
        {
            Accounts = new SelectList(_accountApplication.GetAccounts(), "Id", "FullName");
            Orders = _orderApplication.Search(searchModel);
        }

        [NeedsPermission(ShopPermissions.ConfirmOrders)]
        public IActionResult OnGetConfirm(long id)
        {
            _orderApplication.PaymentSucceeded(id, 0);
            return RedirectToPage("./Index");
        }

        [NeedsPermission(ShopPermissions.CancelOrders)]
        public IActionResult OnGetCancel(long id)
        {
            _orderApplication.Cancel(id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetChecked(long id)
        {
            _orderApplication.Checked(id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeliver(long id)
        {
            _orderApplication.Deliverd(id);
            return RedirectToPage("./Index");
        }

        [NeedsPermission(ShopPermissions.ListItemOrder)]
        public IActionResult OnGetItems(long id)
        {
            var items = _orderApplication.GetItems(id);
            return Partial("Items", items);
        }

        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<OrderViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "Orders.xlsx");
            return File(
                fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "OrdersReport.xlsx"
            );
        }
    }
}
