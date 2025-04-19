using _01_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;
using System.Collections.Generic;
using System.Linq;

namespace _01_LampShadeQuery.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopContext _context;

        public ProductCategoryQuery(ShopContext context)
        {
            _context = context;
        }

        public List<ProductCategoryQueryModel> GetProductCategories()
        {
            return _context.ProductCategories.Where(x => !x.IsRemoved).Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id ,
                Name = x.Name ,
                Picture = x.Picture ,
                PictureAlt = x.PictureAlt ,
                PictureTitle = x.PictureTitle ,
                SubText = x.SubText ,
                Slug = x.Slug
            }).AsNoTracking().ToList();
        }
    }
}
