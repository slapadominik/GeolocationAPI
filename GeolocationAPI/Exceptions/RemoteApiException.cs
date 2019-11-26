using System;

namespace GeolocationAPI.Exceptions
{
    public class RemoteApiException : Exception
    {
        
        public RemoteApiException(string message) : base(message)
        {
        }

        public RemoteApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}