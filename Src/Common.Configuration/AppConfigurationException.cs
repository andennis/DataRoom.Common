using System;

namespace Common.Configuration
{
    public class AppConfigurationException : Exception
    {
        public AppConfigurationException(string message)
            : base(message)
        {
        }
        public AppConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
