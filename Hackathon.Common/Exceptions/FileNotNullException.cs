using System;

namespace Hackathon.Common.Exceptions
{
    public class FileNotNullException : Exception
    {
        public FileNotNullException(string message, Exception exception = null) : base(message, exception)
        {
        }
    }
}
