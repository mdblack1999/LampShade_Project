﻿using System.Linq;
using System.Reflection;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceHost
{
    public class SecurityPageFilter : IPageFilter
    {
        private readonly IAuthHelper _authHelper;

        public SecurityPageFilter(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var handlerPermission =
                (NeedsPermissionAttribute) context.HandlerMethod.MethodInfo.GetCustomAttribute(
                    typeof(NeedsPermissionAttribute));

            if (handlerPermission == null)
                return;

            var accountPermissions = _authHelper.GetPermissions();

            if (accountPermissions.All(x => x != handlerPermission.PermissionCode))
                context.HttpContext.Response.Redirect("/AccessDenied");
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {

        }
    }
}
