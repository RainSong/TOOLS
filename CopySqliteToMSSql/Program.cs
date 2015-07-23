using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


namespace CopySqliteToMSSql
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqliteConnectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
            var sqliteConnection = DataHelper.GetSqliteConnection(sqliteConnectionString);

            var sqlConnectionString = ConfigurationManager.ConnectionStrings["mssql"].ConnectionString;
            var sqlConnection = DataHelper.GetSqlConnection(sqlConnectionString);

            //CopyUrls(sqliteConnection, sqlConnection);
            string[] tableNames = new string[]
            {
                "url","tag","page_tag","page_info","page_file","file_info"
            };
            foreach (string tableName in tableNames)
            {

                try
                {
                    Copy(sqliteConnection, sqlConnection, tableName);
                    Console.WriteLine("表" + tableName + "数据复制成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("发生错误，表" + tableName + "数据复制失败！");
                    LogManager.LogError("发生错误，表" + tableName + "数据复制失败！", ex);
                }
            }
            Console.WriteLine("复制完成，按任意键退出... ...");
            Console.ReadKey();
        }

        static IDataReader GetReader(DbConnection con, string tableName)
        {
            string sql = "select * from " + tableName;
            return DataHelper.GetReader(con, sql);
        }

        static void Copy(DbConnection sqliteCon, SqlConnection sqlCon, string tableName)
        {
            try
            {
                var reader = GetReader(sqliteCon, tableName);
                DataHelper.CopyData(reader, sqlCon, tableName);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }


    }
}
