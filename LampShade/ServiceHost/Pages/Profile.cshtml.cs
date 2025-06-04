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
        public EditProfileViewModel Profile { get; set; }

        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;

        public ProfileModel(IAccountApplication accountApplication, IAuthHelper authHelper)
        {
            _accountApplication = accountApplication;
            _authHelper = authHelper;
        }

        public void OnGet()
        {
            var userId = _authHelper.CurrentAccountId();
            Profile = _accountApplication.GetProfile(userId);
        }
        public IActionResult OnPost(EditProfileViewModel command)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = _accountApplication.EditProfile(command);
            if (!result.IsSucceeded)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }
            return RedirectToPage("/profile",result); 
        }
    }
}
