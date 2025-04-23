using _01_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ProductCategoryModel : PageModel
    {
        public ProductCategoryQueryModel prodctCategory;    
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProductCategoryModel(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public void OnGet(string id)
        {
            prodctCategory = _productCategoryQuery.GetProductCategoryWithProductsBy(id);
        }
    }
}
