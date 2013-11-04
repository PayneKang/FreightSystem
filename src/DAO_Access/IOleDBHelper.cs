using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace DAO_Access
{
    public interface IOleDBHelper
    {
        DataSet ExecuteSql2DataSet(string sql, OleDbParameter[] parameters, int startIndex, int length, string srcTable);
        int ExecuteNonQuery(string sql, OleDbParameter[] parameters = null);
        object ExecuteScalar(string sql, OleDbParameter[] parameters = null);
    }
}
