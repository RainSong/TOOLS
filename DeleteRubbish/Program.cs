using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DeleteRubbish
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.LogInfo("开始清理垃圾文件");
            Console.WriteLine("开始清理垃圾文件");
            string basePath = ConfigurationManager.AppSettings["BasePath"];
            if (string.IsNullOrEmpty(basePath))
            {
                LogManager.LogInfo("根目录为空，无法执行清理");
                LogManager.LogError("根目录为空，无法执行清理");
                Console.WriteLine("根目录为空，无法执行清理");
            }

            string deletePath = ConfigurationManager.AppSettings["DeletePath"];
            if (string.IsNullOrEmpty(deletePath))
            {
                LogManager.LogInfo("要删除的路径为空，无法执行清理");
                LogManager.LogError("要删除的路径为空，无法执行清理");
                Console.WriteLine("要删除的路径为空，无法执行清理");
            }
            string[] deletePaths = deletePath.Split(';');

            DeletePath(basePath, deletePaths);

            LogManager.LogInfo("垃圾文件清理完成");
            Console.WriteLine("垃圾文件清理完成");
            LogManager.LogInfo("===========================================================");

        }

        public static void DeletePath(string basePath, string[] deletePaths)
        {
            if (!Directory.Exists(basePath))
            {
                string msg = string.Format("根目录{0}不存在，无法执行清理", basePath);
                LogManager.LogInfo(msg);
                LogManager.LogError(msg);
                Console.WriteLine(msg);
                return;
            }

            basePath = RemoveLastChar(basePath);

            foreach (string deletePath in deletePaths)
            {
                DeletePath(basePath, deletePath);
            }
        }
        public static void DeletePath(string basePath, string deletePath)
        {
            string path = Path.Combine(basePath, deletePath);
            path = RemoveLastChar(path);
            if (string.IsNullOrEmpty(deletePath))
            {
                string msg = string.Format("目录{0}无法删除", deletePath);
                LogManager.LogInfo(msg);
                LogManager.LogError(msg);
                Console.WriteLine(msg);
                return;
            }
            if (path.Trim().ToLower().Equals(basePath.ToLower()))
            {
                string msg = string.Format("目录{0}无法删除", deletePath);
                LogManager.LogInfo(msg);
                LogManager.LogError(msg);
                Console.WriteLine(msg);
                return;
            }
            if (!Directory.Exists(path))
            {
                string msg = string.Format("目录{0}不存在，无法删除", deletePath);
                LogManager.LogInfo(msg);
                LogManager.LogError(msg);
                Console.WriteLine(msg);
                return;
            }
            try
            {
                Directory.Delete(path, true);
                string msg = string.Format("目录{0}删除成功", deletePath);
                LogManager.LogInfo(msg);
                Console.WriteLine(msg);
            }
            catch (Exception ex)
            {
                string msg = string.Format("目录{0}删除失败", deletePath);
                LogManager.LogError(msg, ex);
                Console.WriteLine(msg);
            }
        }

        public static string RemoveLastChar(string path)
        {

            if (string.IsNullOrEmpty(path)) return path;

            if (path.EndsWith("\\"))
            {
                path = path.Remove(path.LastIndexOf("\\"), 1);
            }
            return path;
        }
    }
}
