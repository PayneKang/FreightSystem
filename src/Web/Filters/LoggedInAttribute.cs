using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreightSystem.Logics.Interfaces;
using FreightSystem.Logics.Implementations;
using FreightSystem.Models;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class LoggedInAttribute : ActionFilterAttribute
    {
        IUserCacheProvider provider = new UserCacheProvider();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserModel loggedUser = provider.GetCurrentLoggedUser();
            if (loggedUser == null)
                filterContext.HttpContext.Response.Redirect("/Security/Login");
            filterContext.Controller.ViewBag.LoggedUser = loggedUser;
            base.OnActionExecuting(filterContext);
        }
    }
}