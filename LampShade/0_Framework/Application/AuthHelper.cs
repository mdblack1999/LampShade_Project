using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _0_Framework.Application
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private const string ViewedCookieName = "ViewedArticles";

        public AuthHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public AuthViewModel CurrentAccountInfo()
        {
            var result = new AuthViewModel();
            if (!IsAuthenticated())
                return result;

            var claims = _contextAccessor.HttpContext.User.Claims.ToList();

            result.Id = long.Parse(claims.FirstOrDefault(x => x.Type == "AccountId")?.Value);
            result.Username = claims.FirstOrDefault(x => x.Type == "Username")?.Value;
            result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            result.Fullname = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            result.Role = Roles.GetRoleBy(result.RoleId);
            result.Picture = claims.FirstOrDefault(x => x.Type == "Picture")?.Value;
            return result;
        }

        public List<int> GetPermissions()
        {
            if (!IsAuthenticated())
                return new List<int>();

            var permissions = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "permissions")
                ?.Value;
            return JsonConvert.DeserializeObject<List<int>>(permissions);
        }

        public long CurrentAccountId()
        {
            return IsAuthenticated()
                ? long.Parse(_contextAccessor.HttpContext.User.Claims.First(x => x.Type == "AccountId").Value)
                : 0;
        }

        public string CurrentAccountMobile()
        {
            return IsAuthenticated()
                ? _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Mobile").Value
                : "";
        }

        public void MarkArticleAsViewed(long articleId)
        {
            var http = _contextAccessor.HttpContext;
            var existing = http.Request.Cookies[ViewedCookieName] ?? "";
            var viewedList = new List<string>();
            if (!string.IsNullOrWhiteSpace(existing))
                viewedList = existing.Split(',').Select(x => x.Trim()).ToList();

            var idStr = articleId.ToString();
            if (!viewedList.Contains(idStr))
                viewedList.Add(idStr);

            var secureCookieValue = string.Join(",", viewedList);

            http.Response.Cookies.Append(
                ViewedCookieName,
                secureCookieValue,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(2)
                });
        }

        public bool HasViewedArticle(long articleId)
        {
            var http = _contextAccessor.HttpContext;
            var cookieValue = http.Request.Cookies[ViewedCookieName] ?? "";
            if (string.IsNullOrWhiteSpace(cookieValue))
                return false;

            var viewedList = cookieValue.Split(',').Select(x => x.Trim()).ToList();
            return viewedList.Contains(articleId.ToString());
        }

        public string GetClientIp()
        {
            var http = _contextAccessor.HttpContext;

            var forwarded = http.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwarded))
            {
                var ip = forwarded.Split(',').First().Trim();
                return ip;
            }

            return http.Connection.RemoteIpAddress?.ToString() ?? "";
        }

        public string CurrentAccountRole()
        {
            if (IsAuthenticated())
                return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return null;
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
            // OR =>
            ////var claims = _contextAccessor.HttpContext.User.Claims.ToList();
            ////if (claims.Count > 0)
            ////    return true;
            ////return false;

        }

        public void SignIn(AuthViewModel account)
        {
            var permissions = JsonConvert.SerializeObject(account.Permissions);
            var claims = new List<Claim>
            {
                new Claim("AccountId", account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Fullname),
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                new Claim("Username", account.Username),// Or Use ClaimTypes.NameIdentifier
                new Claim("Picture", account.Picture),
                new Claim("permissions",permissions),
                new Claim("Mobile",account.Mobile)
            };

            var claimsIdentity = new ClaimsIdentity(claims , CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
            };

            _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme ,
                new ClaimsPrincipal(claimsIdentity) ,
                authProperties).Wait();

            var ip = GetClientIp();
            if (!string.IsNullOrWhiteSpace(ip))
            {
                _contextAccessor.HttpContext.Response.Cookies.Append(
                    "UserIP",
                    ip,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(2)
                    });
            }
        }

        public void SignOut()
        {
            _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}