using System;
using System.Collections.Generic;
using System.Linq;
using _01_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems { get; set; }
        public const string CookieName = "cart-items";
        private readonly IProductQuery _productQuery;

        public CartModel(IProductQuery productQuery)
        {
            CartItems = new List<CartItem>();
            _productQuery = productQuery;
        }

        public void OnGet()
        {
            var json = Request.Cookies[CookieName] ?? "[]";
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(json);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            CartItems = _productQuery.CheckInventoryStatus(cartItems);
        }

        public IActionResult OnGetGoToCheckOut()
        {
            var json = Request.Cookies[CookieName] ?? "[]";
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(json);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            CartItems = _productQuery.CheckInventoryStatus(cartItems);

            //if (CartItems.Any(x => !x.IsInStock))
            //    return RedirectToPage("/Cart");
            //return RedirectToPage("/Checkout");

            //OR :
            return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/Checkout");
        }
    }
}