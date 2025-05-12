using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.Configure.Permission;

namespace ServiceHost.Areas.Administration.Pages.Comments
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<CommentViewModel> Comments;
        public CommentSearchModel SearchModel { get; set; }
        private readonly ICommentApplication _commentApplication;

        public IndexModel(ICommentApplication commentApplication)
        {
            _commentApplication = commentApplication;
        }

        [NeedsPermission(CommentPermission.ListComments)]
        public void OnGet(CommentSearchModel searchModel)
        {
            Comments = _commentApplication.Search(searchModel);
            SearchModel = searchModel;
        }

        [NeedsPermission(CommentPermission.ChangeStatusComments)]
        public IActionResult OnGetConfirm(long id)
        {
            var result = _commentApplication.ChangeStatus(id , CommentViewModel.CommentStatus.Confirmed);
            if (result.IsSuccedded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
        [NeedsPermission(CommentPermission.ChangeStatusComments)]
        public IActionResult OnGetCancel(long id)
        {
            var result = _commentApplication.ChangeStatus(id , CommentViewModel.CommentStatus.Canceled);
            if (result.IsSuccedded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedsPermission(CommentPermission.ChangeStatusComments)]
        public IActionResult OnGetSpam(long id)
        {
            var result = _commentApplication.ChangeStatus(id , CommentViewModel.CommentStatus.Spam);
            if (result.IsSuccedded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedsPermission(CommentPermission.ChangeStatusComments)]
        public IActionResult OnGetPending(long id)
        {
            var result = _commentApplication.ChangeStatus(id , CommentViewModel.CommentStatus.Pending);
            if (result.IsSuccedded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
