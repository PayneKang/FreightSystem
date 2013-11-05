using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace DAO_Access
{
    public class AccessDBHelper:IOleDBHelper
    {
        public static string OleConnString { get; set; }
        static AccessDBHelper()
        {
            OleConnString = string.Format(ConfigurationManager.AppSettings["AccessConnectionString"], ConfigurationManager.AppSettings["DBPath"], ConfigurationManager.AppSettings["DBPassword"]);
        }
        private OleDbConnection conn;

        private void InitDBConnection(){
            this.conn = new OleDbConnection(OleConnString);
        }

        private void OpenDB(){
            this.conn.Open();
        }

        private void CloseDB(){
            this.conn.Close();
        }

        public AccessDBHelper(){
            InitDBConnection();
        }

        public DataSet ExecuteSql2DataSet(string sql, OleDbParameter[] parameters ,int startIndex , int length ,string srcTable)
        {
            OpenDB();
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            if(parameters != null)
                cmd.Parameters.AddRange(parameters);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            if(length == 0 || string.IsNullOrEmpty(srcTable))
                da.Fill(ds);
            else
                da.Fill(ds,startIndex,length,srcTable);
            CloseDB();
            return ds;
        }
        public int ExecuteNonQuery(string sql,OleDbParameter[] parameters = null)
        {
            OpenDB();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            if(parameters!=null)
                cmd.Parameters.AddRange(parameters);
            int count = cmd.ExecuteNonQuery();
            CloseDB();
            return count;
        }
        public object ExecuteScalar(string sql, OleDbParameter[] parameters = null)
        {
            OpenDB();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            object result = cmd.ExecuteScalar();
            CloseDB();
            return result;
        }
    }
}
