using _01_LampShadeQuery;
using _01_LampShadeQuery.Contracts.ArticleCategory;
using _01_LampShadeQuery.Contracts.Product;
using _01_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;
        private readonly IProductQuery _productQuery;

        public MenuViewComponent(IProductCategoryQuery productCategoryQuery , IArticleCategoryQuery articleCategoryQuery ,
            IProductQuery productQuery)
        {
            this._productCategoryQuery = productCategoryQuery;
            _articleCategoryQuery = articleCategoryQuery;
            _productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var result = new MenuModel
            {
                ArticleCategories = _articleCategoryQuery.GetArticleCategories() ,
                ProductCategories = _productCategoryQuery.GetProductCategories() ,
                ProductsView = _productQuery.Search(string.Empty)
            };
            return View(result);
        }
    }
}
