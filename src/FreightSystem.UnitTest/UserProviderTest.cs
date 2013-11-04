using FreightSystem.Logics.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FreightSystem.Models;
using System.Collections.Generic;
using Moq;
using DAO_Access;
using System.Data.OleDb;
using System.Data;

namespace FreightSystem.UnitTest
{
    
    
    /// <summary>
    ///这是 UserProviderTest 的测试类，旨在
    ///包含所有 UserProviderTest 单元测试
    ///</summary>
    [TestClass()]
    public class UserProviderTest
    {

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private DataTable BuildEmptyUserTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("userid");
            return dt;
        }

        /// <summary>
        ///FindUser 的测试
        ///</summary>
        [TestMethod()]
        public void FindUserWithNullDataSet()
        {
            Mock<UserProvider> mockUser = new Mock<UserProvider>();
            Mock<IOleDBHelper> mockDB = new Mock<IOleDBHelper>();
            DataSet mockResult = null;
            OleDbParameter[] mockParameters = new OleDbParameter[] { 
                new OleDbParameter("userid","001"),
                new OleDbParameter("password","001")};
            mockDB.Setup(x => x.ExecuteSql2DataSet(@"select * from users where userid=@userid and password=@password",mockParameters , 0, 0, string.Empty)).Returns(mockResult);

            mockUser.Setup(x => x.dbHelper).Returns(mockDB.Object);
            mockUser.SetReturnsDefault(mockUser.Object);
            
            UserProvider target = mockUser.Object; // TODO: 初始化为适当的值
            
            string userID = "001"; // TODO: 初始化为适当的值
            string password = "001"; // TODO: 初始化为适当的值
            UserModel expected = null; // TODO: 初始化为适当的值
            UserModel actual;
            actual = target.FindUser(userID, password);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///QueryUsers 的测试
        ///</summary>
        [TestMethod()]
        public void QueryUsersTest()
        {
            UserProvider target = new UserProvider(); // TODO: 初始化为适当的值
            int startIndex = 0; // TODO: 初始化为适当的值
            int length = 0; // TODO: 初始化为适当的值
            List<UserModel> expected = null; // TODO: 初始化为适当的值
            List<UserModel> actual;
            actual = target.QueryUsers(startIndex, length);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateUser 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateUserTest()
        {
            UserProvider target = new UserProvider(); // TODO: 初始化为适当的值
            UserModel user = null; // TODO: 初始化为适当的值
            target.UpdateUser(user);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///UserProvider 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void UserProviderConstructorTest()
        {
            UserProvider target = new UserProvider();
            Assert.IsNotNull(target);

        }
    }
}
