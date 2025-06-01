using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.Configure.Permission;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages.Comments
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<CommentViewModel> Comments;

        [BindProperty]
        public string TableJson { get; set; }

        public CommentSearchModel SearchModel { get; set; }

        private readonly ICommentApplication _commentApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(ICommentApplication commentApplication, IReportExporter reportExporter)
        {
            _commentApplication = commentApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(CommentPermission.ListComments)]
        public void OnGet()
        {
            Comments = _commentApplication.Search(SearchModel);
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

        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<CommentViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "Comments.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CommentsReport.xlsx");
        }
    }
}
