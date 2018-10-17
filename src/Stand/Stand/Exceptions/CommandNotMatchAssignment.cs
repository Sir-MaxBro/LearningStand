using System;
using System.Runtime.Serialization;

namespace Stand.UI.Exceptions
{
    [Serializable]
    public class CommandNotMatchAssignment : ApplicationException
    {
        public CommandNotMatchAssignment()
        {
        }

        public CommandNotMatchAssignment(string message)
            : base(message) { }

        public CommandNotMatchAssignment(string message, Exception innerException)
            : base(message, innerException) { }

        protected CommandNotMatchAssignment(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
