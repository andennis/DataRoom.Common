using System;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Common.Logging
{
    public class Log4NetLogger : ILogger
    {
        private static ILog Log { get; set; }

        public static Log4NetLogger GetLoggingService()
        {
            XmlConfigurator.Configure();
            Log = LogManager.GetLogger(typeof(Logger));
            return new Log4NetLogger();
        }

        public void Warn(string message)
        {
            Log.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            Log.Warn(message, ex);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            Log.Error(message, ex);
        }

        public void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public void Fatal(string message, Exception ex)
        {
            Log.Fatal(message, ex);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            Log.Debug(message, ex);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Info(string message, Exception ex)
        {
           Log.Info(message, ex);
        }
    }
}
