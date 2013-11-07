using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreightSystem.Models;
using FreightSystem.Logics.Interfaces;
using FreightSystem.Logics.Implementations;
using Web.Filters;

namespace Web.Controllers
{
    public class SecurityController : Controller
    {
        #region Properties
        IUserCacheProvider cacheProvider = new UserCacheProvider();
        IUserProvider userProvider = new UserProvider();
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            UserModel user = userProvider.FindUser(loginModel.Username, loginModel.Password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "登陆失败，用户名或者密码错误";
                return View();
            }
            cacheProvider.SaveUser(user);
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        [LoggedIn]
        public ActionResult Logout()
        {
            cacheProvider.CleanUser(); 
            return RedirectToAction("Login", "Security");
        }
        #endregion

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
