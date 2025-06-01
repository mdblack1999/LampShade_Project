using System.Collections.Generic;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _01_LampShadeQuery.Contracts.ReportingManagement.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages.Blog.Articles
{
    public class IndexModel : PageModel
    {
        public List<ArticleViewModel> Articles;
        public ArticleSearchModel SearchModel { get; set; }
        public SelectList ArticleCategories;

        [BindProperty]
        public string TableJson { get; set; }

        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;
        private readonly IReportExporter _reportExporter;

        public IndexModel(IArticleApplication articleApplication , IArticleCategoryApplication articleCategoryApplication , IReportExporter reportExporter)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
            _reportExporter = reportExporter;
        }
        [NeedsPermission(BlogPermission.ListArticles)]
        public void OnGet()
        {
            ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories() , "Id" , "Name");
            Articles = _articleApplication.Search(SearchModel);
        }
        public IActionResult OnPostExportToExcel()
        {
            var data = JsonConvert.DeserializeObject<List<ArticleViewModel>>(TableJson);
            var fileContent = _reportExporter.ExportToExcel(data, "Articles.xlsx");
            return File(fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ArticlesReport.xlsx");
        }
    }
}
