using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class AccountModel : PageModel
    {
        [TempData]
        public string LoginMessage { get; set; }

        [TempData]
        public string RegisterMessage { get; set; }

        [TempData]
        public bool RegisterSucceeded { get; set; }

        [BindProperty]
        public Login Login { get; set; }

        [BindProperty]
        public RegisterAccount RegisterModel { get; set; }

        private readonly IAccountApplication _accountApplication;

        public AccountModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostLogin()
        {
            var result = _accountApplication.Login(Login);
            if (result.IsSucceeded)
            {

                return RedirectToPage("/Index");
            }

            LoginMessage = result.Message;
            return Page();

        }


        public IActionResult OnGetLogout()
        {
            _accountApplication.Logout();
            return RedirectToPage("/Account");
        }

        public IActionResult OnPostRegister()
        {
            var result = _accountApplication.Register(RegisterModel);
            RegisterMessage = result.Message;
            RegisterSucceeded = result.IsSucceeded;

            if (!ModelState.IsValid || !result.IsSucceeded)
                return Page();

            RegisterModel = null;
            return Page();
        }
    }
}