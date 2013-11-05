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
    public class UserProvider:IUserProvider
    {
        public const string QueryUserSql = @"select * from users where userid=@userid and password=@password";
        public virtual IOleDBHelper dbHelper { get; private set; }

        public UserProvider()
        {
        }

        public void InitDBHelper()
        {
            dbHelper = new AccessDBHelper();
        }

        public UserModel FindUser(string userID, string password)
        {
            if (dbHelper == null)
                InitDBHelper();
            DataSet ds = dbHelper.ExecuteSql2DataSet(QueryUserSql, new OleDbParameter[] { 
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
            if (dbHelper == null)
                InitDBHelper();
            object count = dbHelper.ExecuteScalar("select count(*) from Users");
            totalCount = (int)count;
            DataSet ds = dbHelper.ExecuteSql2DataSet("select * from Users", null, startIndex, length, "Users");

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
    }
}
