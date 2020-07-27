using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;


namespace LogBook.Business
{
    public class OpenIdAuthorize:AuthorizeAttribute
    {
       
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var returnUrl = string.Empty;
            if (filterContext.HttpContext.Request.Url != null)
                returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "UnauthorizedUser"}));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = returnUrl }));
            }
        }
        //protected override void HandleUnauthorizedRequest(AuthorizationContext ctx)
        //{
        //    if (!ctx.HttpContext.User.Identity.IsAuthenticated)
        //        base.HandleUnauthorizedRequest(ctx);
        //    else
        //    {
        //        ctx.Result = new ViewResult { ViewName = "Error", ViewBag = { message = "Unauthorized." } };
        //        ctx.HttpContext.Response.StatusCode = 403;
        //    }
        //}

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (filterContext == null)
        //        throw new ArgumentNullException(nameof(filterContext));
        //    if (OutputCacheAttribute.IsChildActionCacheActive((ControllerContext)filterContext))
        //        //throw new InvalidOperationException(MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache);
        //        if ((filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ? 1 : (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ? 1 : 0)) != 0)
        //            return;
        //    if (this.AuthorizeCore(filterContext.HttpContext))
        //    {
        //        HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
        //        cache.SetProxyMaxAge(new TimeSpan(0L));
        //        //cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), (object)null);
        //    }
        //    else
        //        this.HandleUnauthorizedRequest(filterContext);
        //}

        //    protected override bool AuthorizeCore(HttpContextBase httpContext)
        //    {
        //        if (httpContext == null)
        //            throw new ArgumentNullException(nameof(httpContext));
        //        IPrincipal user = httpContext.User;
        //        var isUser = user.IsInRole("User");
        //        return false;//user.Identity.IsAuthenticated && (this._users.Length == 0 || ((IEnumerable<string>)this._usersSplit).Contains<string>(user.Identity.Name, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)) && (this._rolesSplit.Length == 0 || ((IEnumerable<string>)this._rolesSplit).Any<string>(new Func<string, bool>(user.IsInRole))));
        //    }
    }
}