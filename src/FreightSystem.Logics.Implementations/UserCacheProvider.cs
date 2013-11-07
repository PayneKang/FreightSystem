using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Logics.Interfaces;
using System.Web;
using FreightSystem.Models;

namespace FreightSystem.Logics.Implementations
{
    public class UserCacheProvider:IUserCacheProvider
    {
        private const string UserSessionKey = "LoggedUser";
        public UserModel GetCurrentLoggedUser()
        {
            object userObj = HttpContext.Current.Session[UserSessionKey];
            if (userObj == null)
                return null;

            return (UserModel)userObj;

        }

        public void SaveUser(UserModel user)
        {
            if (HttpContext.Current.Session[UserSessionKey] == null)
            {
                HttpContext.Current.Session.Add(UserSessionKey, user);
                return;
            }
            HttpContext.Current.Session[UserSessionKey] = user;
        }


        public void CleanUser()
        {
            if (HttpContext.Current.Session[UserSessionKey] == null)
            {
                return;
            }
            HttpContext.Current.Session.Remove(UserSessionKey);
        }
    }
}
