using System;

namespace GeolocationAPI.Exceptions
{
    public class EntityDuplicateException : Exception
    {
        public EntityDuplicateException(string message) : base(message)
        {
        }

        public EntityDuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}