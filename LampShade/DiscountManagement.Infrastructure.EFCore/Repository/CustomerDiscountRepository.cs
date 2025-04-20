using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EfCore;
using System.Collections.Generic;
using System.Linq;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class CustomerDiscountRepository : RepositoryBase<long , CustomerDiscount>, ICustomerDsicountRepository
    {
        private readonly DiscountContext _context;
        private readonly ShopContext _shopContext;

        public CustomerDiscountRepository(DiscountContext context , ShopContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }

        public EditCustomerDiscount GetDetails(long id)
        {
            return _context.CustomerDiscounts.Select(x => new EditCustomerDiscount
            {
                Id = x.Id ,
                ProductId = x.ProductId ,
                DiscountRate = x.DiscountRate ,
                StartDate = x.StartDate.ToString() ,
                EndDate = x.EndDate.ToString() ,
                Reason = x.Reason

            }).FirstOrDefault(x => x.Id == id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
            var product = _shopContext.Products.Select(x => new { x.Id , x.Name }).AsNoTracking().ToList();
            var query = _context.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
            {
                Id = x.Id ,
                StartDate = x.StartDate.ToFarsi() ,
                EndDate = x.EndDate.ToFarsi() ,
                StartDateGr = x.StartDate ,
                EndDateGr = x.EndDate ,
                DiscountRate = x.DiscountRate ,
                ProductId = x.ProductId ,
                Reason = x.Reason ,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
            {
                query = query.Where(x => x.StartDateGr > searchModel.StartDate.ToGeorgianDateTime());
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
            {
                query = query.Where(x => x.EndDateGr < searchModel.EndDate.ToGeorgianDateTime());
            }

            var discount = query.OrderByDescending(x => x.Id).AsNoTracking().ToList();
            discount.ForEach(discount => discount.Product = product.FirstOrDefault(x => x.Id == discount.Id)?.Name);

            return discount;

        }
    }
}
