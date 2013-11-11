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
    public class UserProvider : BaseProvider,IUserProvider
    {
        private const string QueryUserSql = @"select * from users where [userid]=@userid and [password]=@password";
        private const string ChangePasswordSql = @"update users set [password]=@newpassword where ([userid]=@userid)";

        public UserModel FindUser(string userID, string password)
        {
            DataSet ds = DbHelper.ExecuteSql2DataSet(QueryUserSql, new OleDbParameter[] { 
                new OleDbParameter("userid",userID),
                new OleDbParameter("password",password)
            },0,0,string.Empty);

            if (ds == null) 
                return null;

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            return new UserModel()
            {
                 Comment = ds.Tables[0].Rows[0]["comment"].ToString(),
                 CreateDateTime = (DateTime)ds.Tables[0].Rows[0]["createdatetime"],
                 Location = ds.Tables[0].Rows[0]["location"].ToString(),
                 Name = ds.Tables[0].Rows[0]["name"].ToString(),
                 Password = ds.Tables[0].Rows[0]["password"].ToString(),
                 RoleID = (int)ds.Tables[0].Rows[0]["roleid"],
                 UserID = ds.Tables[0].Rows[0]["userid"].ToString()
            };
        }

        public void UpdateUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> QueryUsers(int startIndex, int length,out int totalCount)
        {
            object count = DbHelper.ExecuteScalar("select count(*) from Users");
            totalCount = (int)count;
            DataSet ds = DbHelper.ExecuteSql2DataSet("select * from Users", null, startIndex, length, "Users");

            DataRow[] rows = new DataRow[ds.Tables[0].Rows.Count];
            ds.Tables[0].Rows.CopyTo(rows, 0);
            return (from x in rows
                    select new UserModel()
                    {
                        Comment = x["comment"].ToString(),
                        CreateDateTime = (DateTime)x["createdatetime"],
                        Location = x["location"].ToString(),
                        Name = x["name"].ToString(),
                        Password = x["password"].ToString(),
                        RoleID = (int)x["roleid"],
                        UserID = x["userid"].ToString()
                    }).ToList<UserModel>();
        }


        public void ChangePassword(string userID, string newPassword)
        {
            DbHelper.ExecuteNonQuery(ChangePasswordSql, new OleDbParameter[] { 
                new OleDbParameter("newpassword",newPassword),
                new OleDbParameter("userid",userID)
            });
        }
    }
}
