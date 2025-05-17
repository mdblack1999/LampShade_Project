using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        long PlaceOrder(Cart cart);
        void PaymentSucceeded(long orderId , long refId);

        //double GetAmountBy(long id);
        //void Cancel(long id);
        //List<OrderItemViewModel> GetItems(long orderId);
        //List<OrderViewModel> Search(OrderSearchModel searchModel);
    }
}