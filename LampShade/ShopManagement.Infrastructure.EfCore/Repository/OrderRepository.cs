using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Infrastructure.EfCore;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<long , Order>, IOrderRepository
    {
        private readonly ShopContext _context;
        private readonly AccountContext _accountContext;

        public OrderRepository(ShopContext context , AccountContext accountContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
        }

        public double GetAmountBy(long id)
        {
            var order = _context.Orders
                .Select(x => new { x.PayAmount , x.Id })
                .FirstOrDefault(x => x.Id == id);
            if (order != null)
                return order.PayAmount;
            return 0;
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            var products = _context.Products.AsNoTracking().Select(x => new { x.Id , x.Name }).ToList();
            var order = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == orderId);
            if (order == null)
                return new List<OrderItemViewModel>();
            var items = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id ,
                Count = x.Count ,
                DiscountRate = x.DiscountRate ,
                OrderId = x.OrderId ,
                ProductId = x.ProductId ,
                UnitPrice = x.UnitPrice
            }).ToList();

            foreach (var item in items)
            {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
                var discountAmount = item.UnitPrice * item.DiscountRate / 100;
                item.TotalAmount = (item.UnitPrice - discountAmount) * item.Count;
            }

            return items;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            var accounts = _accountContext.Accounts.AsNoTracking().Select(x => new { x.Id , x.FullName }).ToList();
            var query = _context.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id ,
                AccountId = x.AccountId ,
                DiscountAmount = x.DiscountAmount ,
                IsCanceled = x.IsCanceled ,
                IsPaid = x.IsPaid ,
                IssueTrackingNo = x.IssueTrackingNo ,
                PayAmount = x.PayAmount ,
                PaymentMethodId = x.PaymentMethod ,
                RefId = x.RefId ,
                Pending = x.Pending ,
                Checked = x.Checked ,
                Delivered = x.Delivered ,
                TotalAmount = x.TotalAmount ,
                CreationDate = x.CreationDate.ToFarsi()
            });

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0) query = query.Where(x => x.AccountId == searchModel.AccountId);

            var orders = query.OrderByDescending(x => x.Id).AsNoTracking().ToList();
            foreach (var order in orders)
            {
                order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.FullName;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
            }

            return orders;
        }

        public List<OrderViewModel> GetAllOrders()
        {
            return _context.Orders.Include(x => x.Items).AsNoTracking().Select(x => new OrderViewModel
            {
                Id = x.Id ,
                AccountId = x.AccountId ,
                DiscountAmount = x.DiscountAmount ,
                IsCanceled = x.IsCanceled ,
                IsPaid = x.IsPaid ,
                IssueTrackingNo = x.IssueTrackingNo ,
                PayAmount = x.PayAmount ,
                PaymentMethodId = x.PaymentMethod ,
                RefId = x.RefId ,
                TotalAmount = x.TotalAmount ,
                CreationDate = x.CreationDate.ToFarsi() ,
                Pending = x.Pending ,
                Delivered = x.Delivered ,
                Checked = x.Checked ,
            }).ToList();
        }

        public OrderViewModel GetOrderBy(long id)
        {
            var orders = _context.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id ,
                IsCanceled = x.IsCanceled ,
                Pending = x.Pending ,
                TotalAmount = x.TotalAmount ,
                CreationDate = x.CreationDate.ToFarsi() ,
                AccountId = x.AccountId ,
                Delivered = x.Delivered ,
                IsPaid = x.IsPaid ,
                PayAmount = x.PayAmount ,
                PaymentMethod = x.PaymentMethod.ToString() ,
                RefId = x.RefId ,
                IssueTrackingNo = x.IssueTrackingNo ,
                DiscountAmount = x.DiscountAmount ,

            }).FirstOrDefault(x => x.Id == id);

            if (orders == null)
                return new OrderViewModel();
            return orders;
        }
    }
}