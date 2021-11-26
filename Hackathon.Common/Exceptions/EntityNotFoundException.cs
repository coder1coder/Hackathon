using System;

namespace Hackathon.Common.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public EntityNotFoundException(string message, Exception exception = null) : base(message, exception)
        {
        }
    }
}