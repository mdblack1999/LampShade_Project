﻿using _0_Framework.Application;
using _01_LampShadeQuery.Contracts.Product;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_LampShadeQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        private readonly ShopContext _context;

        public ProductQuery(ShopContext context , InventoryContext inventoryContext , DiscountContext discountContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }


        public List<ProductQueryModel> GetLatestArrivals()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId , x.UnitPrice , }).AsNoTracking().ToList();
            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate , x.ProductId }).AsNoTracking().ToList();
            var products = _context.Products.Include(x => x.Category).AsSplitQuery()
                 .Select(product => new ProductQueryModel
                 {
                     Id = product.Id ,
                     Category = product.Category.Name ,
                     Name = product.Name ,
                     Picture = product.Picture ,
                     PictureAlt = product.PictureAlt ,
                     PictureTitle = product.PictureTitle ,
                     Slug = product.Slug
                 }).AsNoTracking().OrderByDescending(x => x.Id).Take(6).ToList();
            foreach (var product in products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return products;
        }

        public List<ProductQueryModel> Search(string value)
        {

            var inventory = _inventoryContext.Inventory
                .Select(x => new { x.ProductId , x.UnitPrice , }).AsNoTracking().ToList();
            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate , x.ProductId , x.EndDate }).AsNoTracking().ToList();

            var query = _context.Products.Include(x => x.Category).AsSplitQuery()
                 .Select(product => new ProductQueryModel
                 {
                     Id = product.Id ,
                     Category = product.Category.Name ,
                     CategorySlug = product.Category.Slug ,
                     Name = product.Name ,
                     Picture = product.Picture ,
                     PictureAlt = product.PictureAlt ,
                     ShortDescription = product.ShortDescription,
                     PictureTitle = product.PictureTitle ,
                     Slug = product.Slug
                 }).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(value))
                query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));

            List<ProductQueryModel> products = query.OrderByDescending(x => x.Id).AsNoTracking().ToList();

            foreach (var product in products)
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
            return products;
        }
    }
}
