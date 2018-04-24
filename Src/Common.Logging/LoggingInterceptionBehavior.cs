using Microsoft.Practices.Unity.InterceptionExtension;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.Logging
{
    public class LoggingInterceptionBehavior : IInterceptionBehavior
    {
        private readonly ILogger _logger;
        public LoggingInterceptionBehavior(ILogger logger)
        {
            _logger = logger;
        }
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (getNext == null) 
                throw new ArgumentNullException("getNext");

            //Check NoLog attribute
            //bool isNoLogAttr = false;
            //isNoLogAttr = input
            //      .Target
            //      .GetType()
            //      .GetMethods()
            //      .Where(m => m.Name == input.MethodBase.Name)
            //      .FirstOrDefault()
            //      .CustomAttributes
            //      .FirstOrDefault(x => x.AttributeType.Name == "NoLog") != null;

            //if(isNoLogAttr)
            //{
            //    return getNext()(input, getNext);
            //}

            string className = input.Target.GetType().Name;
            if (LogManager.IsLoggingEnabled())
            {
                WriteLog(className, string.Format("Invoking method {0}.{1}", className, input.MethodBase.Name), LogLevel.Trace);
            }

            var sw = Stopwatch.StartNew();

            var result = getNext()(input, getNext);

            if (LogManager.IsLoggingEnabled())
            {
                if (result.Exception != null)
                {
                    //WriteLog(className,
                    //    String.Format("Method {0}.{1} threw exception: {2}", className, input.MethodBase.Name, result.Exception.Message),
                    //    LogLevel.Error);

                    WriteLog(className,
                        string.Format("Method {0}.{1} has executed with exception.", className, input.MethodBase.Name), 
                        LogLevel.Error);
                }
                else
                {
                    WriteLog(className, String.Format("Method {0}.{1} has successfully executed. Executing time: {2}", className, input.MethodBase.Name, sw.ElapsedMilliseconds),
                        LogLevel.Trace);
                }
            }

            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        private void WriteLog(string className, string message, LogLevel logLevel)
        {
            switch (logLevel.Name)
            {
                case "Debug": _logger.Debug(message); break;
                case "Fatal": _logger.Fatal(message); break;
                case "Error": _logger.Error(message); break;
                case "Info": _logger.Info(message); break;
                case "Warn": _logger.Warn(message); break;
                default: _logger.Debug(message); break;
            }
        }
    }
}