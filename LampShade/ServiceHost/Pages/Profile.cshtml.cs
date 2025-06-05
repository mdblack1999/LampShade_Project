using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public EditProfileViewModel Profile { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;

        public ProfileModel(IAccountApplication accountApplication, IAuthHelper authHelper)
        {
            _accountApplication = accountApplication;
            _authHelper = authHelper;
        }
        private void ReloadProfileFromService()
        {
            var userId = _authHelper.CurrentAccountId();
            Profile = _accountApplication.GetProfile(userId);
        }

        public void OnGet()
        {
            ReloadProfileFromService();
        }

        public IActionResult OnPost()
        {
            var userId = _authHelper.CurrentAccountId();
            if (Profile == null)
            {
                ReloadProfileFromService();
                ErrorMessage = "خطای داخلی: اطلاعات پروفایل پیدا نشد.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                ReloadProfileFromService();
                ErrorMessage = "لطفاً فرم را به‌درستی پر کنید";
                return Page();
            }

            var result = _accountApplication.EditProfile(Profile);
            if (!result.IsSucceeded)
            {
                ReloadProfileFromService();
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }

            SuccessMessage = result.Message;
            ReloadProfileFromService();
            return Page();
        }
    }
}
