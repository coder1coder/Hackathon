using System;

namespace Hackathon.Common.Exceptions
{
    public class FileInfoNotFoundException : Exception
    {
        public FileInfoNotFoundException(string message, Exception exception = null) : base(message, exception)
        {
        }
    }
}
