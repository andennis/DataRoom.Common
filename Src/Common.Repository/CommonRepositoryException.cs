using System;

namespace Common.Repository
{
    public class CommonRepositoryException : Exception
    {
        public CommonRepositoryException(string message)
            : base(message)
        {
        }

        public CommonRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
