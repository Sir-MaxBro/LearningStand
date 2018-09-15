using System;

namespace Stand.Domain.Exceptions
{
    public class CommandNotFoundException : ApplicationException
    {
        public CommandNotFoundException(string message)
            : base(message)
        { }
    }
}
