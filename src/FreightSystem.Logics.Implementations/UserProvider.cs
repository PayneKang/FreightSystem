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
                    AreaID = user.AreaID,
                    Area = new BusinessAreaModel(){
                         AreaName = user.BusinessArea.AreaName,
                         ID = user.BusinessArea.ID
                    },
                    Name = user.Name,
                    Password = user.Password,
                    RoleID = user.RoleId,
                    UserID = user.UserID,
                    LastLoginIP = user.LastLoginIP,
                    LastLoginTime = user.LastLoginTime,
                    Role = new RoleModel()
                    {
                        RoleID = user.Roles.RoleID,
                        RoleName = user.Roles.RoleName,
                        Menus = (from x in user.Roles.MenuAccess
                                 orderby x.MenuItem.OrderIndex
                                 select new MenuItemModel()
                                 {
                                     Link = x.MenuItem.Link,
                                     MenuCode = x.MenuItem.MenuCode,
                                     MenuText = x.MenuItem.MenuText,
                                     OrderIndex = x.MenuItem.OrderIndex
                                 }).ToList()

                    }
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
                target.AreaID = user.AreaID;
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


        public RoleListModel GetRoleList()
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                RoleListModel result = new RoleListModel();
                result.ItemList = (from x in context.Roles
                                   select new RoleModel()
                                   {
                                       RoleID = x.RoleID,
                                       RoleName = x.RoleName,
                                       Menus = (from menu in x.MenuAccess
                                                orderby menu.MenuItem.OrderIndex
                                                select new MenuItemModel()
                                                {
                                                    Link = menu.MenuItem.Link,
                                                    MenuCode = menu.MenuItem.MenuCode,
                                                    MenuText = menu.MenuItem.MenuText,
                                                    OrderIndex = menu.MenuItem.OrderIndex
                                                }).ToList()
                                   }).ToList();
                return result;
            }
        }


        public void InsertRoleModel(RoleModel role)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Roles r = new Roles()
                {
                    RoleName = role.RoleName,
                };
                r.MenuAccess = new System.Data.Linq.EntitySet<MenuAccess>();
                r.MenuAccess.AddRange(
                    from x in role.Menus
                    select new MenuAccess()
                    {
                        MenuCode = x.MenuCode,
                        RoleID = r.RoleID
                    }
                    );
                context.Roles.InsertOnSubmit(r);
                context.SubmitChanges();
            }
        }


        public UserListModel GetUserList()
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                UserListModel model = new UserListModel();
                model.ItemList = (from x in context.Users
                                  select new UserModel()
                                  {
                                      Comment = x.Comment,
                                      CreateDateTime = x.CreateDateTime,
                                      LastLoginIP = x.LastLoginIP,
                                      LastLoginTime = x.LastLoginTime,
                                      AreaID = x.AreaID,
                                      Area = new BusinessAreaModel()
                                      {
                                          AreaName = x.BusinessArea.AreaName,
                                          ID = x.BusinessArea.ID
                                      },
                                      Name = x.Name,
                                      Password = x.Password,
                                      RoleID = x.RoleId,
                                      UserID = x.UserID,
                                      Role = new RoleModel()
                                      {
                                          RoleID = x.Roles.RoleID,
                                          RoleName = x.Roles.RoleName
                                      }
                                  }).ToList();
                return model;
            }
        }


        public void InsertUserModel(UserModel user)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Users newUser = new Users()
                {
                    Comment = string.IsNullOrEmpty(user.Comment)? " ":user.Comment,
                    CreateDateTime = DateTime.Now,
                    LastLoginIP = string.Empty,
                    LastLoginTime = DateTime.Now,
                    AreaID = user.AreaID,
                    Name = user.Name,
                    Password = user.Password,
                    RoleId = user.RoleID,
                    UserID = user.UserID,
                    
                };
                context.Users.InsertOnSubmit(newUser);
                context.SubmitChanges();
            }
        }


        public List<MenuItemModel> GetAllMenuItem()
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                return (
                        from x in context.MenuItem
                        orderby x.OrderIndex
                        select new MenuItemModel()
                        {
                            Link = x.Link,
                            MenuText = x.MenuText,
                            MenuCode = x.MenuCode,
                            OrderIndex = x.OrderIndex
                        }
                       ).ToList();
            }
        }
    }
}
