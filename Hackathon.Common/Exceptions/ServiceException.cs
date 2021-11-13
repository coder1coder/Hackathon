using System;

namespace Hackathon.Common.Exceptions
{
    public class ServiceException: Exception
    {
        public ServiceException(string message, Exception exception = null): base(message, exception)
        {
        }
    }
}