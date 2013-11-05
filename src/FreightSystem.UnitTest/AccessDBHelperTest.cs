using DAO_Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Data.OleDb;

namespace FreightSystem.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for AccessDBHelperTest and is intended
    ///to contain all AccessDBHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccessDBHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            File.Copy(@"D:\Projects\FreightSystem\trunk\src\FreightSystem.UnitTest\Database\BaseDB.mdb", @"D:\Projects\FreightSystem\trunk\src\FreightSystem.UnitTest\Database\SysDB.mdb", true);
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }
        //
        #endregion


        /// <summary>
        ///A test for ExecuteSql2DataSet
        ///</summary>
        [TestMethod()]
        public void ExecuteSql2DataSetTestNoData()
        {
            DAO_Access.AccessDBHelper target = new DAO_Access.AccessDBHelper();
            string sql = "select * from users where userid=''";
            System.Data.OleDb.OleDbParameter[] parameters = null;
            int startIndex = 0;
            int length = 5;
            string srcTable = string.Empty;
            System.Data.DataSet actual;
            actual = target.ExecuteSql2DataSet(sql, parameters, startIndex, length, srcTable);
            Assert.AreEqual(0, actual.Tables[0].Rows.Count);
        }

        [TestMethod()]
        public void ExecuteSql2DataSetTestOneRecord()
        {
            DAO_Access.AccessDBHelper target = new DAO_Access.AccessDBHelper();
            string sql = "select * from users where userid=@userid";
            System.Data.OleDb.OleDbParameter[] parameters = new OleDbParameter[]{
                new OleDbParameter(){
                     ParameterName = "userid",
                     Value="Payne"
                }
            };
            int startIndex = 0;
            int length = 0;
            string srcTable = string.Empty;
            System.Data.DataSet actual;
            actual = target.ExecuteSql2DataSet(sql, parameters, startIndex, length, srcTable);
            Assert.AreEqual(1, actual.Tables[0].Rows.Count);
            Assert.AreEqual("Payne", actual.Tables[0].Rows[0]["UserID"].ToString());
            Assert.AreEqual("Payne Kang", actual.Tables[0].Rows[0]["Name"].ToString());
        }

        [TestMethod()]
        public void ExecuteSql2DataSetTestMultRecord()
        {
            DAO_Access.AccessDBHelper target = new DAO_Access.AccessDBHelper();
            string sql = "select * from users";
            System.Data.OleDb.OleDbParameter[] parameters = null;
            int startIndex = 0;
            int length = 5;
            string srcTable = "user";
            System.Data.DataSet actual;
            actual = target.ExecuteSql2DataSet(sql, parameters, startIndex, length, srcTable);
            Assert.AreEqual(2, actual.Tables[0].Rows.Count);
        }

        /// <summary>
        ///A test for ExecuteScalar
        ///</summary>
        [TestMethod()]
        public void ExecuteScalarTest()
        {
            DAO_Access.AccessDBHelper target = new DAO_Access.AccessDBHelper(); // TODO: Initialize to an appropriate value
            string sql = "select userid from users where userid=@userid";
            System.Data.OleDb.OleDbParameter[] parameters = new OleDbParameter[]{
                new OleDbParameter(){
                     ParameterName = "userid",
                     Value="Payne"
                }
            };
            object expected = "Payne"; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.ExecuteScalar(sql, parameters);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            DAO_Access.AccessDBHelper target = new DAO_Access.AccessDBHelper(); // TODO: Initialize to an appropriate value
            string sql = "update users set lastloginip='127.0.0.1'"; // TODO: Initialize to an appropriate value
            System.Data.OleDb.OleDbParameter[] parameters = null; // TODO: Initialize to an appropriate value
            int expected = 2; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.ExecuteNonQuery(sql, parameters);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ExecuteNonQueryTestWithParameters()
        {
            DAO_Access.AccessDBHelper target = new DAO_Access.AccessDBHelper(); // TODO: Initialize to an appropriate value
            string sql = "update users set lastloginip='127.0.0.1' where userid=@userid"; // TODO: Initialize to an appropriate value

            System.Data.OleDb.OleDbParameter[] parameters = new OleDbParameter[]{
                new OleDbParameter(){
                     ParameterName = "userid",
                     Value="Payne"
                }
            };

            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.ExecuteNonQuery(sql, parameters);
            Assert.AreEqual(expected, actual);
        }
    }
}
