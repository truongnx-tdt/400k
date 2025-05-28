using System;
using System.Web;
using System.Web.Mvc;

public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    private readonly string[] allowedRoles;
    public AuthorizeRoleAttribute(params string[] roles)
    {
        this.allowedRoles = roles;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        bool authorize = false;
        var userRole = Convert.ToString(httpContext.Session["Role"]);
        foreach (var role in allowedRoles)
        {
            if (role == userRole)
            {
                authorize = true;
                break;
            }
        }
        return authorize;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectResult("~/Account/Login");
    }
}
