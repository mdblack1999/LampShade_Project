﻿using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;
using System.Collections.Generic;
using System.Linq;

namespace ShopManagement.Infrastructure.EfCore.Repository
{
    public class ProductPictureRepository : RepositoryBase<long , ProductPicture>, IProductPictureRepository
    {
        private readonly ShopContext _context;

        public ProductPictureRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public EditProductPicture GetDetails(long id)
        {
            return _context.ProductPicture.Select(x => new EditProductPicture
            {
                Id = id ,
                PictureAlt = x.PictureAlt ,
                PictureTitle = x.PictureTitle ,
                ProductId = x.ProductId
            }).FirstOrDefault(x => x.Id == id);
        }

        public ProductPicture GetWithProductAndCategory(long id)
        {
            return _context.ProductPicture.Include(x => x.Product)
                .ThenInclude(x => x.Category)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            var query = _context.ProductPicture.Include(x => x.Product).AsSplitQuery().Select(x => new ProductPictureViewModel
            {
                Id = x.Id ,
                Picture = x.Picture ,
                CreationDate = x.CreationDate.ToFarsi() ,
                Product = x.Product.Name ,
                ProductId = x.ProductId ,
                IsRemoved = x.IsRemoved
            });
            if (searchModel.ProductId != 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);
            return query.OrderByDescending(x => x.Id).AsNoTracking().ToList();
        }
    }
}
