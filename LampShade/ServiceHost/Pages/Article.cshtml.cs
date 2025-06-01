using System.Collections.Generic;
using _0_Framework.Application;
using _01_LampShadeQuery.Contracts.Article;
using _01_LampShadeQuery.Contracts.ArticleCategory;
using BlogManagement.Application.Contracts.Article;
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
        private readonly IAuthHelper _authHelper;
        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        private readonly ICommentApplication _commentApplication;


        public ArticleModel(IArticleQuery articleQuery , IArticleCategoryQuery articleCategoryQuery, ICommentApplication commentApplication, IArticleApplication articleApplication, IAuthHelper authHelper)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
            _commentApplication = commentApplication;
            _articleApplication = articleApplication;
            _authHelper = authHelper;
        }

        public void OnGet(string id)
        {
            
            var articleDetails = _articleQuery.GetArticleDetails(id);
            if (articleDetails == null)
            {
                return;
            }

            if (!_authHelper.HasViewedArticle(articleDetails.Id))
            {
                _articleApplication.IncreaseVisitCount(articleDetails.Id);

                _authHelper.MarkArticleAsViewed(articleDetails.Id);
            }
            Article = articleDetails;
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
