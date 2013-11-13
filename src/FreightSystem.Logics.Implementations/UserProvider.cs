using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Logics.Interfaces;
using FreightSystem.Models;
using DAO_Access;
using System.Data.OleDb;
using System.Data;

namespace FreightSystem.Logics.Implementations
{
    public class UserProvider : IUserProvider
    {
        public UserModel FindUser(string userID, string password)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Users user = context.Users.FirstOrDefault(x => x.UserID == userID && x.Password == password);
                if (user == null)
                    return null;
                return new UserModel()
                {
                    Comment = user.Comment,
                    CreateDateTime = user.CreateDateTime,
                    Location = user.Location,
                    Name = user.Name,
                    Password = user.Password,
                    RoleID = user.RoleId,
                    UserID = user.UserID
                };
            }
        }

        public void UpdateUser(UserModel user)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Users target = context.Users.FirstOrDefault(x => x.UserID == user.UserID);
                if (target == null)
                    throw new ApplicationException("用户不存在");
                target.Comment = user.Comment;
                target.LastLoginIP = user.LastLoginIP;
                target.LastLoginTime = user.LastLoginTime;
                target.Location = user.Location;
                target.Name = user.Name;
                target.RoleId = user.RoleID;
                context.SubmitChanges();
            }
        }

        public List<UserModel> QueryUsers(int startIndex, int length,out int totalCount)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                totalCount = context.Users.Count();
                List<UserModel> result = (from x in context.Users
                                          orderby x.CreateDateTime descending
                                          select new UserModel()
                                          {
                                          }).ToList();
                return result;
            }
        }


        public void ChangePassword(string userID, string newPassword)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Users target = context.Users.FirstOrDefault(x => x.UserID == userID);
                if (target == null)
                    throw new ApplicationException("用户不存在");
                target.Password = newPassword;
                context.SubmitChanges();
            }
        }
    }
}
