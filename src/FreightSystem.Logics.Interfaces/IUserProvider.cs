using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FreightSystem.Models;

namespace FreightSystem.Logics.Interfaces
{
    public interface IUserProvider
    {
        UserModel FindUser(string userID, string password);
        List<UserModel> QueryUsers(int startIndex, int length, out int totalCount);
        void UpdateUser(UserModel user);
        void ChangePassword(string userID, string newPassword);
        RoleListModel GetRoleList();
        void InsertRoleModel(RoleModel role);
        UserListModel GetUserList();
        void InsertUserModel(UserModel user);
        List<MenuItemModel> GetAllMenuItem();
    }
}