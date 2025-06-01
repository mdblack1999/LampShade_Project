using DiscountManagement.Application.Contract.ColleagueDiscount;
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

namespace ServiceHost.Areas.Administration.Pages.Discounts.ColleagueDiscounts
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ColleagueDiscountSearchModel SearchModel { get; set; }
        public List<ColleagueDiscountViewModel> ColleagueDiscounts;
        public SelectList Products;

        [BindProperty]
        public string TableJson { get; set; }

        private readonly IProductApplication _productApplication;
        private readonly IColleagueDiscountApplication _colleagueDiscountApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(IProductApplication productApplication, IColleagueDiscountApplication colleagueDiscountApplication, IReportExporter reportExporter)
        {
            _productApplication = productApplication;
            _colleagueDiscountApplication = colleagueDiscountApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(DiscountPermission.ListColleagueDiscounts)]
        public void OnGet()
        {
            Products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
            ColleagueDiscounts = _colleagueDiscountApplication.Search(SearchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new DefineColleagueDiscount
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create" , command);
        }

        [NeedsPermission(DiscountPermission.DefineColleagueDiscounts)]
        public JsonResult OnPostCreate(DefineColleagueDiscount command)
        {
            var result = _colleagueDiscountApplication.Define(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var colleagueDiscount = _colleagueDiscountApplication.GetDetails(id);
            colleagueDiscount.Products = _productApplication.GetProducts();
            return Partial("Edit" , colleagueDiscount);
        }

        [NeedsPermission(DiscountPermission.EditColleagueDiscounts)]
        public JsonResult OnPostEdit(EditColleagueDiscount command)
        {
            var result = _colleagueDiscountApplication.Edit(command);
            return new JsonResult(result);
        }

        [NeedsPermission(DiscountPermission.ActiveDeActiveColleagueDiscount)]
        public IActionResult OnGetRemove(long id)
        {
            _colleagueDiscountApplication.Remove(id);
            return RedirectToPage("./Index");
        }

        [NeedsPermission(DiscountPermission.ActiveDeActiveColleagueDiscount)]
        public IActionResult OnGetRestore(long id)
        {
            _colleagueDiscountApplication.Restore(id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<ColleagueDiscountViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "ColleagueDis.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ColleagueDiscountReport.xlsx");
        }
    }

}
