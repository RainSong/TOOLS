using System;
using System.Collections.Generic;
using System.Text;

namespace DeleteRubbish
{
    public static class LogManager
    {
        private static log4net.ILog logger;
        private static object _lock = new object();
        public static log4net.ILog GetLogger()
        {
            if (logger == null)
            {
                lock (_lock)
                {
                    if (logger == null)
                    {
                        //string path = System.Web.HttpContext.Current.Server.MapPath("~/log4netConfig.xml");
                        //FileInfo fi = new FileInfo(path);
                        //log4net.Config.XmlConfigurator.Configure(fi);
                        logger = log4net.LogManager.GetLogger("logger");
                    }
                }
            }
            return logger;
        }

        public static void LogError(object ex)
        {
            log4net.ILog logger = LogManager.GetLogger();
            logger.Error(ex);
        }

        public static void LogInfo(object msg)
        {
            log4net.ILog logger = LogManager.GetLogger();
            logger.Info(msg);
        }

        public static void LogError(string messsage, Exception ex)
        {
            log4net.ILog logger = LogManager.GetLogger();
            logger.Error(new Exception(messsage, ex));
        }
    }
}
