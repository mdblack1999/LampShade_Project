using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;
using _01_LampshadeQuery.Contracts;

namespace ServiceHost.Pages
{
    public class CheckoutModel : PageModel
    {
        public Cart Cart { get; set; }
        public const string CookieName = "cart-items";
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutModel(ICartCalculatorService cartCalculatorService)
        {
            _cartCalculatorService = cartCalculatorService;
        }

        public void OnGet()
        {
            var json = Request.Cookies[CookieName] ?? "[]";
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(json);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
        }
    }
}

