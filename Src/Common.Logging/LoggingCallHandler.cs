using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace Common.Logging
{
    public class LoggingCallHandler : ICallHandler
    {      
         private readonly ILogger _logger;
         public LoggingCallHandler(ILogger logger)
        {
            _logger = logger;
        }
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (getNext == null)
                throw new ArgumentNullException(nameof(getNext));

            string className = input.Target.GetType().Name;           
            _logger.Debug($"Invoking method {className}.{input.MethodBase.Name}");

            // Invoke the next behavior in the chain.
            IMethodReturn result = getNext()(input, getNext);
           
            if (result.Exception != null)
                _logger.Error($"Method {className}.{input.MethodBase.Name} has thrown exception: {result.Exception.Message}");
            else
                _logger.Debug($"Method {className}.{input.MethodBase.Name} has been executed successfully");

            return result;
        }

        public int Order { get; set; }

    }
}