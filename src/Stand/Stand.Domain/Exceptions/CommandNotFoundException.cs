﻿using System;
using System.Runtime.Serialization;

namespace Stand.Domain.Exceptions
{
    [Serializable]
    public class CommandNotFoundException : ApplicationException
    {
        public CommandNotFoundException() { }

        public CommandNotFoundException(string message)
            : base(message) { }

        public CommandNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected CommandNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
