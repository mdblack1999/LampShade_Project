using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using AccountManagement.Application.Contracts.Account;
using BlogManagement.Application.Contracts.Article;
using CommentManagement.Application.Contracts.Comment;
using DiscountManagement.Application.Contract.ColleagueDiscount;
using DiscountManagement.Application.Contract.CustomerDiscount;
using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages
{
    public class IndexModel : PageModel
    {
        #region Widgets

        public long AllUserCount { get; set; }
        public long AllArticleCount { get; set; }
        public long AllProductCount { get; set; }
        public long AllOrderCount { get; set; }
        public double TotalSales { get; set; }
        #endregion

        #region Percentage Widgets

        //sales
        public double TodaySales { get; set; }
        public double YesterdaySales { get; set; }
        public int SalesChangePercent { get; set; }

        // Orders
        public int TodayOrdersCount { get; set; }
        public int YesterdayOrdersCount { get; set; }
        public int OrdersChangePercent { get; set; }

        // Products
        public int TodayProductsCount { get; set; }
        public int YesterdayProductsCount { get; set; }
        public int ProductsChangePercent { get; set; }

        // Articles
        public int TodayArticlesCount { get; set; }
        public int YesterdayArticlesCount { get; set; }
        public int ArticlesChangePercent { get; set; }

        // Accounts
        public int TodayUsersCount { get; set; }
        public int YesterdayUsersCount { get; set; }
        public int UsersChangePercent { get; set; }
        #endregion

        #region Chart Property Data

        public List<string> MonthLabels { get; set; } = new();
        public List<double> MonthlyRevenue { get; set; } = new();
        public List<int> MonthlyOrdersCount { get; set; } = new();

        public List<string> PaymentMethodNames { get; set; } = new();
        public List<int> PaymentMethodCounts { get; set; } = new();

        public List<string> MostVisitedArticleTitles { get; set; } = new();
        public List<int> MostVisitedArticleViews { get; set; } = new();


        public List<string> TopCategoryLabels { get; set; } = new();
        public List<int> TopCategoryCount { get; set; } = new();

        public List<string> ProductDistLabels { get; set; } = new();
        public List<int> ProductDistCounts { get; set; } = new();

        public List<string> LowStockProductNames { get; set; } = new();
        public List<long> LowStockCounts { get; set; } = new();

        public List<int> MonthlyUserCount { get; set; } = new();

        public List<string> RoleNames { get; set; } = new();
        public List<int> RoleCounts { get; set; } = new();

        public List<string> DiscountRadarLabels { get; set; } = new();
        public List<double> DiscountCustomerData { get; set; } = new();
        public List<double> DiscountPartnerData { get; set; } = new();

        public List<string> CategorySalesLabels { get; set; } = new();
        public List<double> CategorySalesVolumes { get; set; } = new();
        public List<int> CategorySalesCounts { get; set; } = new();

        public List<string> CommentLabels { get; set; } = new();
        public List<int> CommentCounts { get; set; } = new();

        public List<string> TopProductNames { get; set; } = new();
        public List<int> TopProductSales { get; set; } = new();

        public List<int> MonthlyArticleCount { get; set; } = new();
        public List<string> MonthlyArticleCategories { get; set; } = new();

        public List<int> PendingOrdersCount { get; set; } = new();
        public List<int> PaidOrdersCount { get; set; } = new();
        public List<int> CanceledOrdersCount { get; set; } = new();
        public List<RecentSmsDto> RecentSms { get; set; }
        #endregion

        #region Services

        private readonly IAccountApplication _accountApp;
        private readonly IArticleApplication _articleApp;
        private readonly IProductApplication _productApp;
        private readonly IOrderApplication _orderApp;
        private readonly ICommentApplication _commentApp;
        private readonly ICustomerDiscountApplication _customerDiscountApp;
        private readonly IColleagueDiscountApplication _colleagueDiscountApp;
        private readonly IInventoryApplication _inventoryApp;
        private readonly ISmsService _smsService;

        #endregion

        #region Ctor And Injection
        public IndexModel(
            IAccountApplication accountApp ,
            IArticleApplication articleApp ,
            IProductApplication productApp ,
            IOrderApplication orderApp ,
            ICommentApplication commentApp ,
            ICustomerDiscountApplication customerDiscountApp ,
            IColleagueDiscountApplication colleagueDiscountApp ,
            IInventoryApplication inventoryApp, ISmsService smsService)
        {
            _accountApp = accountApp;
            _articleApp = articleApp;
            _productApp = productApp;
            _orderApp = orderApp;
            _commentApp = commentApp;
            _customerDiscountApp = customerDiscountApp;
            _colleagueDiscountApp = colleagueDiscountApp;
            _inventoryApp = inventoryApp;
            _smsService = smsService;
        }

        #endregion


        public void OnGet()
        {
            #region SmsReport
            RecentSms = _smsService.GetRecentSmsSent();
            #endregion

            #region Widgets
            // Total Prop
            var allProducts = _productApp.GetProducts();
            var allArticles = _articleApp.GetAllArticles();
            var allUsers = _accountApp.GetAccounts();

            AllUserCount = _accountApp.GetAccounts().Count;
            AllArticleCount = _articleApp.GetAllArticles().Count;
            AllProductCount = _productApp.GetProducts().Count;
            var allOrders = _orderApp.GetAllOrders();
            AllOrderCount = allOrders.Count;
            TotalSales = allOrders.Where(o => !o.IsCanceled).Sum(o => o.TotalAmount);

            // from yesterday And Today
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            #endregion

            #region Sales % Change
            TodaySales = allOrders
                .Where(o => o.CreationDate.ToGregorianDateTime().Date == today)
                .Sum(o => o.TotalAmount);

            YesterdaySales = allOrders
                .Where(o => o.CreationDate.ToGregorianDateTime().Date == yesterday)
                .Sum(o => o.TotalAmount);

            SalesChangePercent = YesterdaySales > 0
                ? (int)Math.Round((TodaySales - YesterdaySales) * 100.0 / YesterdaySales)
                : 0;
            #endregion

            #region Orders % Change
            TodayOrdersCount = allOrders.Count(o => o.CreationDate.ToGregorianDateTime().Date == today);
            YesterdayOrdersCount = allOrders.Count(o => o.CreationDate.ToGregorianDateTime().Date == yesterday);
            OrdersChangePercent = YesterdayOrdersCount > 0
                ? (int)Math.Round((TodayOrdersCount - YesterdayOrdersCount) * 100.0 / YesterdayOrdersCount)
                : 0;
            #endregion

            #region Products % Change
            TodayProductsCount = allProducts.Count(p => p.CreationDate.ToGregorianDateTime().Date == today);
            YesterdayProductsCount = allProducts.Count(p => p.CreationDate.ToGregorianDateTime().Date == yesterday);
            ProductsChangePercent = YesterdayProductsCount > 0
                ? (int)Math.Round((TodayProductsCount - YesterdayProductsCount) * 100.0 / YesterdayProductsCount)
                : 0;
            #endregion

            #region Articles % Change
            TodayArticlesCount = allArticles.Count(a => a.PublishDate.ToGregorianDateTime().Date == today);
            YesterdayArticlesCount = allArticles.Count(a => a.PublishDate.ToGregorianDateTime().Date == yesterday);
            ArticlesChangePercent = YesterdayArticlesCount > 0
                ? (int)Math.Round((TodayArticlesCount - YesterdayArticlesCount) * 100.0 / YesterdayArticlesCount)
                : 0;
            #endregion

            #region Users % Change
            TodayUsersCount = allUsers.Count(u => u.CreationDate.ToGregorianDateTime().Date == today);
            YesterdayUsersCount = allUsers.Count(u => u.CreationDate.ToGregorianDateTime().Date == yesterday);
            UsersChangePercent = YesterdayUsersCount > 0
                ? (int)Math.Round((TodayUsersCount - YesterdayUsersCount) * 100.0 / YesterdayUsersCount)
                : 0;
            #endregion

            #region Year Range

            var persianCal = new PersianCalendar();
            var shamsiYear = persianCal.GetYear(DateTime.Now);
            var startDate = persianCal.ToDateTime(shamsiYear , 1 , 1 , 0 , 0 , 0 , 0);
            var endDate = persianCal.ToDateTime(shamsiYear , 12 , 29 , 23 , 59 , 59 , 999);

            #endregion

            #region Orders Filtered
            var pc = new PersianCalendar();

            var orders = allOrders
                .Where(o => !string.IsNullOrWhiteSpace(o.CreationDate))
                .Where(o =>
                {
                    var dt = o.CreationDate.ToGregorianDateTime();
                    return dt >= startDate && dt <= endDate;
                })
                .ToList();
            #endregion

            #region Monthly Revenue & Orders
            var revDict = orders
                .GroupBy(o => pc.GetMonth(o.CreationDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g.Sum(o => o.TotalAmount));

            var ordDict = orders
                .GroupBy(o => pc.GetMonth(o.CreationDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g.Count());

            var persianMonthNames = new[]
            {
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
            };
            MonthLabels.AddRange(persianMonthNames);

            // پر کردن داده‌های ماهانه
            for (int m = 1; m <= 12; m++)
            {
                MonthlyRevenue.Add(revDict.ContainsKey(m) ? revDict[m] : 0);
                MonthlyOrdersCount.Add(ordDict.ContainsKey(m) ? ordDict[m] : 0);
            }
            #endregion

            #region Top Categories Sold
            var orderItems = orders
                .SelectMany(o => _orderApp.GetItems(o.Id))
                .ToList();

            var catSold = orderItems
                .GroupBy(i => _productApp.GetProducts().First(p => p.Id == i.ProductId).Category)
                .Select(g => new { Category = g.Key , Count = g.Sum(i => i.Count) })
                .OrderByDescending(x => x.Count)
                .Take(7)
                .ToList();

            foreach (var c in catSold)
            {
                TopCategoryLabels.Add(c.Category);
                TopCategoryCount.Add(c.Count);
            }

            #endregion

            #region Payment Method Orders
            var allOrderVm = _orderApp.GetAllOrders();

            var paymentMethodGroups = allOrderVm
                .GroupBy(i => i.PaymentMethodId)
                .Select(g => new
                {
                    MethodId = g.Key,
                    Count = g.Count()
                })
                .ToList();

            foreach (var item in paymentMethodGroups)
            {
                var method = PaymentMethod.GetBy(item.MethodId);
                PaymentMethodNames.Add(method?.Name ?? "نامشخص");
                PaymentMethodCounts.Add(item.Count);
            }

            

            #endregion

            #region Product Distribution

            var prodDist = _productApp.GetProducts()
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key , Count = g.Count() })
                .ToList();

            foreach (var d in prodDist)
            {
                ProductDistLabels.Add(d.Category);
                ProductDistCounts.Add(d.Count);
            }

            #endregion

            #region Low Stock

            var lowStock = _inventoryApp.GetAllInventories()
                .OrderBy(i => i.CurrentCount)
                .Take(10)
                .ToList();

            foreach (var inv in lowStock)
            {
                LowStockProductNames.Add(inv.Product);
                LowStockCounts.Add(inv.CurrentCount);
            }

            #endregion

            #region Monthly User Signups

            var users = _accountApp.GetAccounts()
                .Where(u =>
                    !string.IsNullOrWhiteSpace(u.CreationDate) &&
                    u.CreationDate.ToGregorianDateTime() >= startDate &&
                    u.CreationDate.ToGregorianDateTime() <= endDate);

            var usrDict = users
                .GroupBy(u => DateTime.Parse(u.CreationDate).Month)
                .ToDictionary(g => g.Key , g => g.Count());

            for (int m = 1; m <= 12; m++)
                MonthlyUserCount.Add(usrDict.ContainsKey(m) ? usrDict[m] : 0);

            #endregion

            #region Role Distribution

            var roleDist = _accountApp.GetAccounts()
                .GroupBy(u => u.Role)
                .Select(g => new { Role = g.Key , Count = g.Count() })
                .ToList();

            foreach (var r in roleDist)
            {
                RoleNames.Add(r.Role);
                RoleCounts.Add(r.Count);
            }
            #endregion

            #region Comment Stats (Unapproved)

            var unapprovedComments = _commentApp.GetAllComment()
                .Where(c => c.Status == 0)
                .ToList();

            var commentGroups = unapprovedComments
                .GroupBy(c => c.Type)
                .Select(g => new
                {
                    Label = g.Key == 1 ? "گروه محصولات" :
                        g.Key == 2 ? "گروه مقالات" : "سایر" ,
                    Count = g.Count()
                })
                .ToList();

            CommentLabels = commentGroups.Select(x => x.Label).ToList();
            CommentCounts = commentGroups.Select(x => x.Count).ToList();

            #endregion

            #region Discount per Product Chart

            var custDiscounts = _customerDiscountApp.GetAllCustomerDiscount()
                .Where(d =>
                    d.StartDateGr.Date <= today &&
                    d.EndDateGr.Date >= today)
                .ToList();

            var collDiscounts = _colleagueDiscountApp.GetColleagueDiscount()
                .Where(d => !d.IsRemoved)
                .ToList();

            var prodLabels = custDiscounts.Select(d => d.Product)
                .Union(collDiscounts.Select(d => d.Product))
                .Distinct()
                .ToList();

            foreach (var prod in prodLabels)
            {
                DiscountRadarLabels.Add(prod);

                var custRate = custDiscounts
                    .FirstOrDefault(d => d.Product == prod)?
                    .DiscountRate ?? 0;
                DiscountCustomerData.Add(custRate);

                var collRate = collDiscounts
                    .FirstOrDefault(d => d.Product == prod)?
                    .DiscountRate ?? 0;
                DiscountPartnerData.Add(collRate);
            }

            #endregion

            #region Category Sales

            var catSales = orderItems
                .GroupBy(i =>
                    _productApp.GetProducts().First(p => p.Id == i.ProductId).Category
                )
                .Select(g => new
                {
                    Category = g.Key ,
                    TotalVolume = g.Sum(i => i.Count * i.UnitPrice) ,
                    TotalCount = g.Sum(i => i.Count)
                })
                .ToList();

            foreach (var cs in catSales)
            {
                CategorySalesLabels.Add(cs.Category);
                CategorySalesVolumes.Add(cs.TotalVolume);
                CategorySalesCounts.Add(cs.TotalCount);
            }

            #endregion

            #region Top Products

            var prodSales = orderItems
                .GroupBy(i => i.ProductId)
                .Select(g => new { Id = g.Key , Count = g.Sum(i => i.Count) })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();

            var products = _productApp.GetProducts();
            foreach (var ps in prodSales)
            {
                var p = products.First(x => x.Id == ps.Id);
                TopProductNames.Add(p.Name);
                TopProductSales.Add(ps.Count);
            }

            #endregion

            #region Monthly Articles Published

            var published = _articleApp.GetAllArticles()
                .Where(a =>
                {
                    var dt = a.PublishDate.ToGregorianDateTime();
                    return dt >= startDate && dt <= endDate;
                })
                .ToList();

            var artDict = published
                .GroupBy(a => pc.GetMonth(a.PublishDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g.Count());

            var artCatDict = published
                .GroupBy(a => pc.GetMonth(a.PublishDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g
                    .GroupBy(x => x.Category)
                    .OrderByDescending(gg => gg.Count())
                    .First().Key);

            for (int m = 1; m <= 12; m++)
            {
                MonthlyArticleCount.Add(artDict.ContainsKey(m) ? artDict[m] : 0);
                MonthlyArticleCategories.Add(artCatDict.ContainsKey(m) ? artCatDict[m] : "نامشخص");
            }

            #endregion

            #region Orders Status Over Months

            var pendingDict = orders
                .Where(o => !o.IsPaid && !o.IsCanceled)
                .GroupBy(o => pc.GetMonth(o.CreationDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g.Count());

            var paidDict = orders
                .Where(o => o.IsPaid && !o.IsCanceled)
                .GroupBy(o => pc.GetMonth(o.CreationDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g.Count());

            var canceledDict = orders
                .Where(o => o.IsCanceled)
                .GroupBy(o => pc.GetMonth(o.CreationDate.ToGregorianDateTime()))
                .ToDictionary(g => g.Key , g => g.Count());

            for (int m = 1; m <= 12; m++)
            {
                PendingOrdersCount.Add(pendingDict.ContainsKey(m) ? pendingDict[m] : 0);
                PaidOrdersCount.Add(paidDict.ContainsKey(m) ? paidDict[m] : 0);
                CanceledOrdersCount.Add(canceledDict.ContainsKey(m) ? canceledDict[m] : 0);
            }

            #endregion

            #region Top 10 Visited Articles
            var top10ArticleList = _articleApp.GetAllArticles()
                .OrderByDescending(a => a.VisitCount)
                .Take(10)
                .Select(a => new { a.Title, a.VisitCount })
                .ToList();

            foreach (var a in top10ArticleList)
            {
                MostVisitedArticleTitles.Add(a.Title);
                MostVisitedArticleViews.Add(a.VisitCount);
            }
            #endregion

        }
    }

    #region Chart Body Class

    public class Chart
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<int> Data { get; set; }

        [JsonProperty(PropertyName = "backgroundColor")]
        public string[] BackgroundColor { get; set; }

        [JsonProperty(PropertyName = "borderColor")]
        public string BorderColor { get; set; }
    }

    #endregion

}
