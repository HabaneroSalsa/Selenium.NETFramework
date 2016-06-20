using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using Dapper;
using System.Collections.Generic;

namespace OnlineStore.TestDataAccess
{
    class ExcelDataAccess
    {
        public static string TestDataFileConnection()
        {
            var fileName = ConfigurationManager.AppSettings["TestDataSheetPath"];
            var con = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = {0}; Extended Properties=Excel 12.0;", fileName);
            return con;
        }

        public static UserData GetTestUserData(string keyName)
        {
            using (var connection = new OleDbConnection(TestDataFileConnection()))
            {
                connection.Open();
                var query = string.Format("select * from [Credentials$] where key='{0}'", keyName);
                var value = connection.Query<UserData>(query).FirstOrDefault();
                connection.Close();
                return value;
            }
        }

        public static List<ShoppingData> GetTestCaseData()
        {
            using (var connection = new OleDbConnection(TestDataFileConnection()))
            {
                connection.Open();
                var query = string.Format("select * from [ShoppingItems$]");
                var value = connection.Query<ShoppingData>(query).ToList();
                connection.Close();
                return value;
            }
        }
    }
}
