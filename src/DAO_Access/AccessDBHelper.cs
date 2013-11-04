using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace DAO_Access
{
    public class AccessDBHelper
    {
        public static string OleConnString { get; set; }
        static AccessDBHelper()
        {
            OleConnString = ConfigurationManager.ConnectionStrings["AccessConnectionString"].ConnectionString;
        }
        public DataSet ExecuteSql2DataSet(string sql,int startIndex = 0, int length = 0)
        {
            throw new NotImplementedException();
        }
    }
}
