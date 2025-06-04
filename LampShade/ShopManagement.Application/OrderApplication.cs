using System.Collections.Generic;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmsService _smsService;
        private readonly IShopInventoryAcl _shopInventoryAcl;
        private readonly IShopAccountAcl _shopAccountAcl;

        public OrderApplication(IOrderRepository orderRepository , IAuthHelper authHelper , IConfiguration configuration , IShopInventoryAcl shopInventoryAcl , ISmsService smsService, IShopAccountAcl shopAccountAcl)
        {
            _orderRepository = orderRepository;
            _authHelper = authHelper;
            _configuration = configuration;
            _shopInventoryAcl = shopInventoryAcl;
            _smsService = smsService;
            _shopAccountAcl = shopAccountAcl;
        }


        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId , cart.PaymentMethod , cart.TotalAmount , cart.DiscountAmount ,
                cart.PayAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id , cartItem.Count , cartItem.UnitPrice , cartItem.DiscountRate);
                order.AddItem(orderItem);
            }
            _orderRepository.Create(order);
            _orderRepository.SaveChanges();
            return order.Id;
        }

        public double GetAmountBy(long id)
        {
            return _orderRepository.GetAmountBy(id);
        }

        public void Cancel(long id)
        {
            var order = _orderRepository.Get(id);
            order.Cancel();
            _orderRepository.SaveChanges();
        }

        public void Checked(long id)
        {
            var order = _orderRepository.Get(id);
            order.MarkAsChecked();
            _orderRepository.SaveChanges();
        }

        public void Deliverd(long id)
        {
            var order = _orderRepository.Get(id);
            order.MarkAsDelivered();
            _orderRepository.SaveChanges();
        }

        public string PaymentSucceeded(long orderId , long refId)
        {
            var order = _orderRepository.Get(orderId);
            order?.SuccessfulPayment(refId);
            var symbol = _configuration["Symbol"];
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            if (order != null)
            {
                order.SetIssueTrackingNo(issueTrackingNo);
                if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";
            }
            _orderRepository.SaveChanges();
            
            // Send SMS
            if (order != null)
            {
                var (name, mobile) = _shopAccountAcl.GetAccountBy(order.AccountId);
                _smsService.Send(mobile,
                    $"{name} عزیز سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و در فرایند ارسال قرار گرفت.");
            }

            return issueTrackingNo;

        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return _orderRepository.Search(searchModel);
        }

        public List<OrderViewModel> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public OrderViewModel GetOrderBy(long id)
        {
            return _orderRepository.GetOrderBy(id);
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            return _orderRepository.GetItems(orderId);
        }
    }
}