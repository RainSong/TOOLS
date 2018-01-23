using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryProcedure
{
    public static class Logger
    {
        private static NLog.Logger ErrorLogger;
        private static NLog.Logger InfoLogger;
        static Logger()
        {
            ErrorLogger = NLog.LogManager.GetLogger("Error2File");
            InfoLogger = NLog.LogManager.GetLogger("Info2File");
        }
        public static void Error(string message, Exception ex)
        {
            ErrorLogger.Error(ex, message);
        }
        public static void Info(string message)
        {
            InfoLogger.Info(message);
        }
    }
}
