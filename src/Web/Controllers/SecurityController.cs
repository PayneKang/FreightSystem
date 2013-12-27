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
        IBusinessProvider businessProvider = new BusinessProvider();
        private const string AllowedPwdCharactors = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        #endregion

        #region Actions
        [LoggedIn(CheckAccess:true,AccessCode:"USERMGR")]
        [HttpGet]
        public ActionResult NewUser()
        {
            ViewBag.AllArea = (from x in businessProvider.ListAllArea()
                               select new SelectListItem()
                               {
                                   Text = x.AreaName,
                                   Value = x.ID.ToString()
                               }).ToList();
            return View();
        }
        [LoggedIn(CheckAccess: true, AccessCode: "USERMGR")]
        [HttpPost]
        public ActionResult NewUser(UserModel model)
        {
            try
            {
                userProvider.InsertUserModel(model);
                return RedirectToAction("UserList", "Security");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "USERMGR")]
        [HttpGet]
        public ActionResult UserList(){
            UserListModel model = userProvider.GetUserList();
            return View(model);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "ROLEMGR")]
        [HttpGet]
        public ActionResult NewRole()
        {
            List<MenuItemModel> menus = userProvider.GetAllMenuItem();
            ViewBag.AllMenu = menus;
            List<FuncItemModel> funcs = userProvider.GetAllFuncItem();
            ViewBag.AllFunc = funcs;
            return View();
        }
        [LoggedIn(CheckAccess: true, AccessCode: "ROLEMGR")]
        [HttpPost]
        public ActionResult NewRole(RoleModel model)
        {
            try
            {
                model.Menus = GetSelectedMenuItem();
                model.AccessList = GetSelectedFuncItem();
                userProvider.InsertRoleModel(model);
                return RedirectToAction("RoleList", "Security");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [LoggedIn(CheckAccess: true, AccessCode: "ROLEMGR")]
        public ActionResult RoleList()
        {
            RoleListModel model = userProvider.GetRoleList();
            List<FuncItemModel> funcs = userProvider.GetAllFuncItem();
            foreach (RoleModel role in model.ItemList)
            {
                foreach(FuncItemModel func in funcs){
                    role.AccessList = role.AccessList.Replace(func.FuncCode, string.Format(" {0} ",func.FuncText));
                }
            }
            return View(model); 
        }
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
            if (user.Role.AccessList.Contains("LOCALRDLIST"))
                return RedirectToAction("LocalTransportRecordList", "Business");
            return RedirectToAction("Index", "Business");
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

        [LoggedIn(CheckAccess: true, AccessCode: "ROLEMGR")]
        [HttpGet]
        public ActionResult RoleMgr()
        {
            int roleID;
            int.TryParse(RouteData.GetRequiredString("id"), out roleID);
            List<MenuItemModel> menus = userProvider.GetAllMenuItem();
            ViewBag.AllMenu = menus;
            List<FuncItemModel> funcs = userProvider.GetAllFuncItem();
            ViewBag.AllFunc = funcs;
            RoleModel role = userProvider.FindRole(roleID);
            return View(role);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "ROLEMGR")]
        [HttpPost]
        public ActionResult RoleMgr(RoleModel model)
        {
            model.Menus = GetSelectedMenuItem();
            model.AccessList = GetSelectedFuncItem();
            userProvider.UpdateRoleModel(model);
            return RedirectToAction("RoleList", "Security");
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

        private List<MenuItemModel> GetSelectedMenuItem()
        {
            string menus = Request["Menus"];
            if (string.IsNullOrEmpty(menus))
                return new List<MenuItemModel>();
            List<MenuItemModel> allmenu = userProvider.GetAllMenuItem();
            if (allmenu == null)
                return new List<MenuItemModel>();
            return (from x in allmenu
                    where menus.Contains(x.MenuCode)
                    select new MenuItemModel()
                    {
                        Link = x.Link,
                        MenuCode = x.MenuCode,
                        MenuText = x.MenuText,
                        OrderIndex = x.OrderIndex
                    }).ToList();
        }

        private string GetSelectedFuncItem()
        {
            return Request["funcs"].Replace(",", "|");
        }
    }
}
