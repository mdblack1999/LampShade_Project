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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public Cart Cart { get; set; }
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        public const string CookieName = "cart-items";
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutModel(ICartCalculatorService cartCalculatorService , ICartService cartService , IProductQuery productQuery , IOrderApplication orderApplication , IZarinPalFactory zarinPalFactory , IAuthHelper authHelper)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
            _orderApplication = orderApplication;
            _zarinPalFactory = zarinPalFactory;
            _authHelper = authHelper;
            Cart = new Cart();
        }

        public IActionResult OnGet()
        {
            var value = Request.Cookies[CookieName];
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value ?? "[]");

            if (cartItems == null || !cartItems.Any())
                return RedirectToPage("/Index");

            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);

            return Page();
        }

        public IActionResult OnPostPay(int paymentMethod)
        {
            var cart = _cartService.Get();
            cart.SetPaymentMethod(paymentMethod);

            var result = _productQuery.CheckInventoryStatus(cart.Items);

            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("/Cart");

            var orderId = _orderApplication.PlaceOrder(cart);
            if (paymentMethod == 1)
            {
                var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
                    cart.PayAmount.ToString(CultureInfo.InvariantCulture) ,
                    mobile: "09016834243" ,
                    email: "Email@gmail.com" ,
                    description: "خرید از درگاه لوازم خانگی و دکوری" ,
                    orderId);

                var redirectUrl = _zarinPalFactory.GetStartPayUrl(paymentResponse.Data.Authority);
                return Redirect(redirectUrl);
            }
            var paymentResult = new PaymentResult();
            return RedirectToPage("/PaymentResult" ,
                paymentResult.Succeeded(
                    "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه، سفارش ارسال خواهد شد." ,
                    null));
        }

        public IActionResult OnGetCallBack([FromQuery] string authority , [FromQuery] string status ,
            [FromQuery] long oId)
        {
            var orderAmount = _orderApplication.GetAmountBy(oId);
            var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority ,
                orderAmount.ToString(CultureInfo.InvariantCulture));

            var result = new PaymentResult();

            if (status == "OK" && verificationResponse.Status >= 100)
            {
                var issueTrackingNo = _orderApplication.PaymentSucceeded(oId , verificationResponse.RefID);
                Response.Cookies.Delete("cart-items");
                result = result.Succeeded("پرداخت با موفقیت انجام شد" , issueTrackingNo);
                return RedirectToPage("/PaymentResult" , result);
            }
            result = result.Failed(
                "پرداخت با شکست مواجه شد. در صورت کسر وجه از حساب، مبلغ تا 24 ساعت آینده به حساب شما بازگردانده خواهد شد");
            return RedirectToPage("/PaymentResult" , result);
        }
    }
}

