using DiscountManagement.Application.Contract.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using DiscountManagement.Configuration.Permission;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using BlogManagement.Application.Contracts.Article;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages.Discounts.CustomerDiscounts
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [BindProperty(SupportsGet = true)]
        public CustomerDiscountSearchModel SearchModel { get; set; }
        public List<CustomerDiscountViewModel> CustomerDiscounts { get; set; }
        public SelectList Products;

        [BindProperty]
        public string TableJson { get; set; }

        private readonly IProductApplication _productApplication;
        private readonly ICustomerDiscountApplication _customerDiscountApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(IProductApplication productApplication , ICustomerDiscountApplication customerDiscountApplication, IReportExporter reportExporter)
        {
            _productApplication = productApplication;
            _customerDiscountApplication = customerDiscountApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(DiscountPermission.ListCustomerDiscounts)]
        public void OnGet()
        {
            Products = new SelectList(_productApplication.GetProducts() , "Id" , "Name");
            CustomerDiscounts = _customerDiscountApplication.Search(SearchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new DefineCustomerDiscount
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create" , command);
        }

        [NeedsPermission(DiscountPermission.DefineCustomerDiscounts)]
        public JsonResult OnPostCreate(DefineCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Define(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var customerDiscount = _customerDiscountApplication.GetDetails(id);
            customerDiscount.Products = _productApplication.GetProducts();
            return Partial("Edit" , customerDiscount);
        }

        [NeedsPermission(DiscountPermission.EditCustomerDiscounts)]
        public JsonResult OnPostEdit(EditCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<CustomerDiscountViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "CustomerDis.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "customerDiscountReport.xlsx");
        }
    }

}
