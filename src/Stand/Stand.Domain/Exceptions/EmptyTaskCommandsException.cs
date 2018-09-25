using System;
using System.Runtime.Serialization;

namespace Stand.Domain.Exceptions
{
    [Serializable]
    public class EmptyTaskCommandsException : ApplicationException
    {
        public EmptyTaskCommandsException() { }

        public EmptyTaskCommandsException(string message)
            : base(message) { }

        public EmptyTaskCommandsException(string message, Exception innerException)
            : base(message, innerException) { }

        protected EmptyTaskCommandsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
