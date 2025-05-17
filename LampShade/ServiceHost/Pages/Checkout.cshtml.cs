using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery.Contracts;
using _01_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class CheckoutModel : PageModel
    {
        public Cart Cart { get; set; }
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        public const string CookieName = "cart-items";
        //private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutModel(ICartCalculatorService cartCalculatorService , ICartService cartService , IProductQuery productQuery , IOrderApplication orderApplication/*, IZarinPalFactory zarinPalFactory*/, IAuthHelper authHelper)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
            _orderApplication = orderApplication;
            //_zarinPalFactory = zarinPalFactory;
            _authHelper = authHelper;
        }

        public void OnGet()
        {
            var json = Request.Cookies[CookieName] ?? "[]";
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(json);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);

            _cartService.Set(Cart);
        }

        //public IActionResult OnGetPay()
        //{
        //    var cart = _cartService.Get();
        //    var result = _productQuery.CheckInventoryStatus(cart.Items);

        //    if (result.Any(x => !x.IsInStock))
        //        return RedirectToPage("/Cart");
        //    var orderId = _orderApplication.PlaceOrder(cart);

        //    var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
        //        cart.PayAmount.ToString(CultureInfo.InvariantCulture) , "" , "" ,
        //        "خرید از درگاه لوازم خانگی و دکوری" , orderId);

        //    return Redirect(
        //        $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
        //}

        //public IActionResult OnGetCallBack([FromQuery] string authority , [FromQuery] string status ,[FromQuery] long oId)
        //{

        //}
    }
}

