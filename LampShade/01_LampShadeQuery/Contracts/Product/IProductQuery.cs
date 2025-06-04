using System.Collections.Generic;
using ShopManagement.Application.Contracts.Order;

namespace _01_LampShadeQuery.Contracts.Product
{
    public interface IProductQuery
    {
        ProductQueryModel GetProductDetails(string slug);
        ProductQueryModel GetProductForCart(long id);
        List<ProductQueryModel> GetLatestArrivals();
        List<ProductQueryModel> Search(string value);
        List<CartItem> CheckInventoryStatus(List<CartItem> cartItems);
    }
}
