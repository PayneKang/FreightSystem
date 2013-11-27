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
        private bool _checkAccess = false;
        private string _accessCode = string.Empty;
        private const string ALLACCESS = "ALL";
        private const string ERR_ACCESS_DENIED = "权限不足";
        public LoggedInAttribute()
        {
        }
        public LoggedInAttribute(bool CheckAccess,string AccessCode):this()
        {
            _checkAccess = true;
            _accessCode = AccessCode;
        }

        IUserCacheProvider provider = new UserCacheProvider();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserModel loggedUser = provider.GetCurrentLoggedUser();
            if (loggedUser == null)
            {
                filterContext.Result = new RedirectResult("/Security/Login");
                return;
            }

            if (!_checkAccess)
            {
                filterContext.Controller.ViewBag.LoggedUser = loggedUser;
                base.OnActionExecuting(filterContext);
                return;
            }

            if (loggedUser.Role.AccessList == null)
            {
                filterContext.Result = new RedirectResult(string.Format("/Error/DisplayError?errmsg={0}", ERR_ACCESS_DENIED));
                return;
            }

            if (loggedUser.Role.AccessList.Contains(_accessCode) || loggedUser.Role.AccessList.Contains(ALLACCESS))
            {
                filterContext.Controller.ViewBag.LoggedUser = loggedUser;
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult(string.Format("/Error/DisplayError?errmsg={0}", ERR_ACCESS_DENIED));
            return;
        }
    }
}