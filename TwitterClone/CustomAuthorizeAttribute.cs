using System;
using System.Web;
using System.Web.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        if (httpContext.User.Identity.IsAuthenticated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        {
            
            filterContext.Result = new RedirectResult("~/Login/Login");
        }
        else
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
