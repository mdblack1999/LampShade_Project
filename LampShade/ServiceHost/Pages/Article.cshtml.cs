using System.Collections.Generic;
using _01_LampShadeQuery.Contracts.Article;
using _01_LampShadeQuery.Contracts.ArticleCategory;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleQueryModel Article;
        public List<ArticleQueryModel> LatestArticles;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        private readonly ICommentApplication _commentApplication;

        public ArticleModel(IArticleQuery articleQuery , IArticleCategoryQuery articleCategoryQuery, ICommentApplication commentApplication)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
            _commentApplication = commentApplication;
        }

        public void OnGet(string id)
        {
            Article = _articleQuery.GetArticleDetails(id);
            LatestArticles = _articleQuery.LatestArticles();
            ArticleCategories = _articleCategoryQuery.GetArticleCategories();
        }

        public IActionResult OnPost(AddComment command , string articleSlug)
        {
            command.Type = CommentType.ArticleType;
            var result = _commentApplication.Add(command);
            return RedirectToPage("/Article" , new { Id = articleSlug });
        }
    }
}
