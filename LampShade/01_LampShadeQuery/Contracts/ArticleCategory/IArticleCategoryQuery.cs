using System.Collections.Generic;

namespace _01_LampShadeQuery.Contracts.ArticleCategory
{
    public interface IArticleCategoryQuery
    {
        ArticleCategoryQueryModel GetArticleCategoryBy(string slug);
        List<ArticleCategoryQueryModel> GetArticleCategories();
    }
}
