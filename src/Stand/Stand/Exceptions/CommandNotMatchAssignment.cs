using System;

namespace Stand.UI.Exceptions
{
    internal class CommandNotMatchAssignment : ApplicationException
    {
        public CommandNotMatchAssignment(string message)
            : base(message)
        { }
    }
}
