using _01_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public IProductCategoryQuery ProductCategoryQuery { get; }

        public MenuViewComponent(IProductCategoryQuery productCategoryQuery)
        {
            ProductCategoryQuery = productCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
