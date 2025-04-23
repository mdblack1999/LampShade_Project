using _01_LampShadeQuery.Contracts.Product;
using _01_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LatestArrivalsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public LatestArrivalsViewComponent(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var Products = _productQuery.GetLatestArrivals();
            return View(Products);
        }
    }
}
