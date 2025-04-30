using _0_Framework.Domain;
using BlogManagement.Application.Contracts.Article;
using System.Collections.Generic;

namespace BlogManagement.Domain.ArticleAgg
{
    public interface IArticleRepository : IRepository<long ,Article>
    {
        EditArticle GetDetails(long id);
        Article GetWitCategory(long id);
        List<ArticleViewModel> Search(ArticleSearchModel searchModel);
    }
}
