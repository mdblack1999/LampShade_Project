using System.Collections.Generic;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<AccountViewModel> Accounts;
        public AccountSearchModel SearchModel;
        public SelectList Roles;

        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IAccountApplication accountApplication , IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        [NeedsPermission(AccountPermission.ListAccounts)]
        public void OnGet(AccountSearchModel searchModel)
        {
            Roles = new SelectList(_roleApplication.List() , "Id" , "Name");
            Accounts = _accountApplication.Search(searchModel);
        }
        public IActionResult OnGetCreate()
        {
            var command = new RegisterAccount
            {
                Roles = _roleApplication.List()
            };
            command.RePassword = command.Password;
            return Partial("./Create" , command);
        }

        [NeedsPermission(AccountPermission.RegisterAccount)]
        public JsonResult OnPostCreate(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var account = _accountApplication.GetDetails(id);
            account.Roles = _roleApplication.List();
            return Partial("Edit" , account);
        }

        [NeedsPermission(AccountPermission.EditAccounts)]
        public JsonResult OnPostEdit(EditAccount command)
        {
            var result = _accountApplication.Edit(command);
            return new JsonResult(result);
        }
        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword { Id = id };
            return Partial("ChangePassword" , command);
        }

        [NeedsPermission(AccountPermission.ChangePassword)]
        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePassword(command);
            return new JsonResult(result);
        }
    }
}
