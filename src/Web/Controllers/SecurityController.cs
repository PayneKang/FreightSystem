﻿using System;
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
        private const string AllowedPwdCharactors = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
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
            if (loginModel == null)
            {
                ViewBag.ErrorMessage = "登陆失败，请输入用户名密码";
                return View();
            }
            if (loginModel.Username == null)
            {
                ViewBag.ErrorMessage = "登陆失败，请输入用户名";
                return View();
            }
            if (loginModel.Password == null)
            {
                ViewBag.ErrorMessage = "登陆失败，请输入密码";
                return View();
            }
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

        [HttpGet]
        [LoggedIn]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult ChangePassword(ChangePasswordModel pwdModel)
        {
            UserModel currentUser = cacheProvider.GetCurrentLoggedUser();
            if (pwdModel.OldPassword == null || currentUser.Password != pwdModel.OldPassword)
            {
                ViewBag.Message = "旧密码输入错误";
                return View();
            }
            if (pwdModel.NewPassword == null || pwdModel.OldPassword == null || pwdModel.NewPassword.Length < 6 || pwdModel.NewPassword.Length >= 32)
            {
                ViewBag.Message = "新密码长度限制为 6 到 32 位";
                return View();
            }
            if(pwdModel.NewPassword != pwdModel.ConfirmPassword){
                ViewBag.Message = "两次新密码输入必须相同";
                return View();
            }
            if (!ValidateString(pwdModel.NewPassword, AllowedPwdCharactors))
            {
                ViewBag.Message = "新密码不符合规定";
                return View();
            }
            try
            {
                userProvider.ChangePassword(currentUser.UserID, pwdModel.NewPassword);
                ViewBag.Message = "密码修改成功";
                currentUser.Password = pwdModel.NewPassword;
                cacheProvider.SaveUser(currentUser);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
            return View();
        }
        #endregion

        private bool ValidateString(string source, string allowCharactors)
        {
            int count = source.Count(x => allowCharactors.Contains(x));
            if (count == source.Count())
                return true;
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Business");
            }
        }
    }
}