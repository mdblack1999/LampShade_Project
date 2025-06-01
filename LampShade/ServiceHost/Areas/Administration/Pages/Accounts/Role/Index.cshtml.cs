using System.Collections.Generic;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<RoleViewModel> Roles;
        [BindProperty]
        public string TableJson { get; set; }

        private readonly IRoleApplication _roleApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(IRoleApplication roleApplication, IReportExporter reportExporter)
        {
            _roleApplication = roleApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(AccountPermission.ListRoles)]
        public void OnGet()
        {
            Roles = _roleApplication.List();
        }
        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<RoleViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "Roles.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "RolesReport.xlsx");
        }
    }
}
