using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Ionic.Zip;
using System.IO;

namespace BackUpWebSite
{
    class Program
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string ConncectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        /// <summary>
        /// 数据库名称
        /// </summary>
        private static string DBName = ConfigurationSettings.AppSettings["DBName"];
        /// <summary>
        /// 要备份的目录
        /// </summary>
        private static string[] Dirs = ConfigurationSettings.AppSettings["Dirs"].Split(';');
        /// <summary>
        /// 备份位置
        /// </summary>
        private static string BackTo = ConfigurationSettings.AppSettings["BackTo"];

        private static string BaseDir = ConfigurationSettings.AppSettings["BaseDir"];
        static void Main(string[] args)
        {
            string timeSting = DateTime.Now.ToString("yyyy年MM月dd日");
            LogManager.LogInfo(string.Format("开始{0}备份", timeSting));
            string path = string.Format(@"{0}\{1}", BackTo, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Console.WriteLine("开始备份");
            LogInfo("开始备份文件");
            try
            {
                BackUpFile(BaseDir, Dirs, path);
                LogInfo("文件备份完成");
            }
            catch (Exception ex)
            {
                LogError("发生错误，文件备份失败", ex);
            }
            LogInfo("开始备份数据库");
            try
            {
                BackUpDB(ConncectionString, DBName, path);
                LogInfo("数据库备份完成");
            }
            catch (Exception ex)
            {
                LogError("发生错误，数据库份失败", ex);
            }
            Console.WriteLine("备份完成");
            LogManager.LogInfo(string.Format("{0}备份完成", timeSting));
            LogManager.LogInfo("===========================================================");
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbName"></param>
        /// <param name="path"></param>
        static void BackUpDB(string connectionString, string dbName, string path)
        {
            string fileName = string.Format(@"{0}\dbback", path);
            string sql = string.Format(@"BACKUP DATABASE {0} 
                                         TO DISK = '{1}'
                                         WITH FORMAT,
                                              MEDIANAME = '{0}{2}',
                                              NAME = 'Full Backup of {0}'", dbName, fileName, DateTime.Now.ToString("yyyyMMddHHmmss"));
            Execute(sql, connectionString);
        }
        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="path"></param>
        static void BackUpFile(string baseDir, string[] dirs, string path)
        {
            string fileName = string.Format(@"{0}\source.zip", path);
            List<string> list = new List<string>();
            //foreach (string dir in dirs)
            //{
            //    list.AddRange(GetFiles(dir));
            //}
            using (ZipFile zf = new ZipFile())
            {
                foreach (string dir in dirs)
                {
                    zf.AddItem(baseDir + dir, dir);
                }
                zf.Save(fileName);
            }
        }
        /// <summary>
        /// 获取一个数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        static SqlConnection GetConnection(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            return con;
        }
        /// <summary>
        /// 执行备份操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        static int Execute(string sql, string connectionString)
        {
            SqlConnection con = GetConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 递归获取目录中的所有文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        static List<string> GetFiles(string dir)
        {
            List<string> list = new List<string>();
            list.AddRange(Directory.GetFiles(dir));
            string[] childDirs = Directory.GetDirectories(dir);
            foreach (string childDir in childDirs)
            {
                list.AddRange(GetFiles(childDir));
            }
            return list;
        }

        static void LogInfo(string msg)
        {
            Console.WriteLine(msg);
            LogManager.LogInfo(msg);
        }

        static void LogError(string msg, Exception ex)
        {
            LogInfo(msg);
            LogManager.LogError(msg, ex);
        }
    }


}
