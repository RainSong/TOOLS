using System;
using System.IO;
using System.Text;

namespace Daemon
{
    class Logger
    {

        public static void Error(string message, Exception ex = null)
        {
            try
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs\\error");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var file = Path.Combine(dir, DateTime.Now.ToString("yyyyMMdd") + ".log");
                var msg = BuilderErrorMessage(message, ex);
                WriteFile(file, msg);
            }
            catch { }
        }



        public static void Info(string message)
        {
            try
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs\\info");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var file = Path.Combine(dir, DateTime.Now.ToString("yyyyMMdd") + ".log");
                var msg = BuilderInfoMessage(message);
                WriteFile(file, msg);
            }
            catch { }
        }

        private static void WriteFile(string filePath, string msg)
        {
            //using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            //{
            File.AppendAllText(filePath, msg);
            //    var bytes = UTF8Encoding.UTF8.GetBytes(msg);
            //    fs.Write(bytes, (int)fs.Length, bytes.Length);
            //}
        }

        private static string BuilderErrorMessage(string message, Exception ex)
        {
            if(ex == null)
            {
                return BuilderInfoMessage(message);
            }
            return string.Format("{0} {1} \r\n{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), message, ex.ToString());
        }

        private static string BuilderInfoMessage(string message)
        {
            return string.Format("{0} {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), message);
        }
    }
}
