using System.Collections.Generic;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;

namespace ServiceHost.Areas.Administration.Pages.Blog.ArticleCategories
{
    public class IndexModel : PageModel
    {
        public List<ArticleCategoryViewModel> ArticleCategories;
        public ArticleCategorySearchModel SearchModel;
        [BindProperty]
        public string TableJson { get; set; }
        private readonly IArticleCategoryApplication _articleCategoryApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(IArticleCategoryApplication articleCategoryApplication, IReportExporter reportExporter)
        {
            _articleCategoryApplication = articleCategoryApplication;
            _reportExporter = reportExporter;
        }

        [NeedsPermission(BlogPermission.ListArticleCategories)]
        public void OnGet(ArticleCategorySearchModel searchModel)
        {
            ArticleCategories = _articleCategoryApplication.Search(searchModel);
        }
        public IActionResult OnGetCreate()  
        {
            return Partial("./Create" , new CreateArticleCategory());
        }

        [NeedsPermission(BlogPermission.CreateArticleCategory)]
        public JsonResult OnPostCreate(CreateArticleCategory command)
        {
            var result = _articleCategoryApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var articleCategory = _articleCategoryApplication.GetDetails(id);
            return Partial("Edit" , articleCategory);
        }

        [NeedsPermission(BlogPermission.EditArticleCategory)]
        public JsonResult OnPostEdit(EditArticleCategory command)
        {
            var result = _articleCategoryApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<ArticleCategoryViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "ArticleCategories.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ArticleCategoriesReport.xlsx");
        }
    }
}
