using NLog;
using System;

namespace Common.Logging
{
    public class NLogLogger : Logger, ILogger
    {

        public static NLogLogger GetLoggingService()
        {
            var logger = (NLogLogger) LogManager.GetLogger("NLogLogger", typeof (NLogLogger));
            return logger;
        }
    }
}