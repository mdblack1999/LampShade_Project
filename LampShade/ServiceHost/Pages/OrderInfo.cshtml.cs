// File: ServiceHost/Pages/OrderInfo.cshtml.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.Order;
using _01_LampShadeQuery.Contracts.Product;
using _0_Framework.Application;

namespace ServiceHost.Pages
{
    [Authorize]
    public class OrderInfoModel : PageModel
    {
        private readonly IOrderApplication _orderApplication;
        private readonly IAuthHelper _authHelper;
        private readonly IProductQuery _productQuery;

        public OrderInfoModel(
            IOrderApplication orderApplication,
            IAuthHelper authHelper,
            IProductQuery productQuery)
        {
            _orderApplication = orderApplication;
            _authHelper = authHelper;
            _productQuery = productQuery;
        }

        public class OrderWithCart
        {
            public OrderViewModel Order { get; set; }
            public Cart Cart { get; set; }
        }

        public List<OrderWithCart> PendingOrders { get; set; } = new();
        public List<OrderWithCart> DeliveredOrders { get; set; } = new();
        public List<OrderWithCart> CanceledOrders { get; set; } = new();

        [BindProperty]
        public long CancelOrderId { get; set; }

        public IActionResult OnGet()
        {
            var userId = _authHelper.CurrentAccountId();

            var allOrders = _orderApplication.GetAllOrders()
                .Where(o => o.AccountId == userId)
                .OrderByDescending(o => o.CreationDate)
                .ToList();

            if (allOrders == null || allOrders.Count == 0)
                return Page();

            foreach (var order in allOrders)
            {
                var cart = new Cart();
                var orderItems = _orderApplication.GetItems(order.Id);

                foreach (var oi in orderItems)
                {
                    var productDto = _productQuery.GetProductForCart(oi.ProductId);
                    var cartItem = new CartItem
                    {
                        Id = oi.ProductId,
                        Name = productDto.Name,
                        Picture = productDto.Picture,
                        UnitPrice = oi.UnitPrice,
                        Count = oi.Count,
                        DiscountRate = oi.DiscountRate,
                        TotalItemPrice = oi.UnitPrice * oi.Count
                    };
                    cartItem.DiscountAmount = cartItem.TotalItemPrice * oi.DiscountRate / 100.0;
                    cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
                    cart.Add(cartItem);
                }

                bool paymentFailed = !order.IsPaid && string.IsNullOrWhiteSpace(order.IssueTrackingNo);
                bool isCanceled = order.IsCanceled || paymentFailed;
                bool isDelivered = order.Delivered && !order.IsCanceled;
                bool isPending =
                    !isCanceled
                    && !order.Delivered
                    && order.IsPaid
                    && (
                        (order.RefId != 0 && !string.IsNullOrWhiteSpace(order.IssueTrackingNo))
                        || (order.RefId == 0 && order.IsPaid)
                    );

                bool isChecked = order.Checked && !order.IsCanceled && order.IsPaid;

                var owc = new OrderWithCart
                {
                    Order = order,
                    Cart = cart
                };

                if (isCanceled)
                    CanceledOrders.Add(owc);
                else if (isDelivered)
                    DeliveredOrders.Add(owc);
                else if (isChecked || isPending)
                    PendingOrders.Add(owc);
            }

            return Page();
        }

        public IActionResult OnPostCancel(long orderId)
        {
            var userId = _authHelper.CurrentAccountId();
            var order = _orderApplication.GetOrderBy(orderId);
            if (order == null || order.AccountId != userId)
                return RedirectToPage();

            var orderDate = order.CreationDate.ToGregorianDateTime();
            var hoursSince = (DateTime.Now - orderDate).TotalHours;
            bool canCancel =
                hoursSince <= 24
                && order.Pending
                && !order.Delivered
                && !order.IsCanceled
                && order.IsPaid
                && !order.Checked;

            if (canCancel)
                _orderApplication.Cancel(orderId);

            return RedirectToPage();
        }
    }
}
