using System.Collections.Generic;

namespace Stand.General.Entities
{
    public class Command
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<Command> Subcommands { get; set; }
    }
}
