using System;
using System.Runtime.Serialization;

namespace Communication.Component.Error
{
    [Serializable]
    class HttpToolsException : Exception
    {
        public HttpToolsException()
        {
        }

        public HttpToolsException(string message) : base(message)
        {
        }

        public HttpToolsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpToolsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}