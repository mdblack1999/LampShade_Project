﻿using _0_Framework.Application;
using _01_LampShadeQuery.Contracts.Product;
using _01_LampShadeQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_LampShadeQuery.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopContext _context;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;

        public ProductCategoryQuery(ShopContext context , InventoryContext inventoryContext , DiscountContext discountContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProductCategoryQueryModel> GetProductCategories()
        {
            return _context.ProductCategories.Where(x => !x.IsRemoved)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id ,
                    Name = x.Name ,
                    Picture = x.Picture ,
                    PictureAlt = x.PictureAlt ,
                    PictureTitle = x.PictureTitle ,
                    SubText = x.SubText ,
                    Slug = x.Slug ,
                    ProductsCount = x.Products.Count ,
                }).AsNoTracking().ToList();
        }

        public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId , x.UnitPrice , }).ToList();
            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate , x.ProductId }).AsNoTracking().ToList();
            var categories = _context.ProductCategories.Include(x => x.Products)
                 .ThenInclude(x => x.Category).AsSplitQuery()
                 .Select(x => new ProductCategoryQueryModel
                 {
                     Id = x.Id ,
                     Name = x.Name ,
                     Products = MapProducts(x.Products)
                 }).AsNoTracking().ToList();

            foreach (var category in categories)
            {
                foreach (var product in category.Products)
                {
                    var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productInventory != null)
                    {
                        var price = productInventory.UnitPrice;
                        product.Price = price.ToMoney();
                        var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount != null)
                        {
                            int discountRate = discount.DiscountRate;
                            product.DiscountRate = discountRate;
                            product.HasDiscount = discountRate > 0;
                            var discountAmount = Math.Round((price * discountRate) / 100);
                            product.PriceWithDiscount = (price - discountAmount).ToMoney();
                        }
                    }
                }
            }

            return categories;
        }

        private static List<ProductQueryModel> MapProducts(List<Product> products)
        {
            return products.Select(product => new ProductQueryModel
            {
                Id = product.Id ,
                Category = product.Category.Name ,
                Name = product.Name ,
                Picture = product.Picture ,
                PictureAlt = product.PictureAlt ,
                PictureTitle = product.PictureTitle ,
                Slug = product.Slug
            }).ToList();
        }

        public ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new { x.ProductId , x.UnitPrice , }).AsNoTracking().ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate , x.ProductId , x.EndDate }).AsNoTracking().ToList();

            var category = _context.ProductCategories.Include(x => x.Products)
                 .ThenInclude(x => x.Category).AsSplitQuery()
                 .Select(x => new ProductCategoryQueryModel
                 {
                     Id = x.Id ,
                     Name = x.Name ,
                     Description = x.Description ,
                     MetaDescription = x.MetaDescription ,
                     Keywords = x.Keywords ,
                     Slug = x.Slug ,
                     Products = MapProducts(x.Products)
                 }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);


            foreach (var product in category.Products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        int discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.DiscountExpiredDate = discount.EndDate.ToDiscountFormat();
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return category;
        }
    }
}
