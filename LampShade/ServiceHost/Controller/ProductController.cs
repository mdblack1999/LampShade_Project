using System.Collections.Generic;
using _01_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductQuery _productQuery;

        public ProductController(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        [HttpGet]
        public List<ProductQueryModel> GetArrivals()
        {
            return _productQuery.GetLatestArrivals();
        }
    }
}
