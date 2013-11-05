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
    }
}