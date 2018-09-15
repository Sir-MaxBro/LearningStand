using System;

namespace Stand.Domain.Exceptions
{
    public class EmptyTaskCommandsException : ApplicationException
    {
        public EmptyTaskCommandsException(string message)
            : base(message) { }
    }
}
