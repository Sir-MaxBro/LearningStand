using Stand.General.Entities;
using Stand.General.Insrastructure.Settings;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Stand.General.Insrastructure.Commands
{
    public class CommandProvider
    {
        public IEnumerable<Command> GetCommands()
        {
            var settingsService = SettingsService.GetInstance();
            var pathToCommands = settingsService.GetSettings().PathToCommands;
            var xdocument = XDocument.Load(pathToCommands);

            var commandElements = xdocument.Elements("Commands").Elements("Command");

            var commands = this.GetMappedCommands(commandElements);

            return commands;
        }

        public IEnumerable<string> GetAllCommandNames()
        {
            var commands = this.GetCommands();
            var commandNames = new List<string>();

            foreach (var command in commands)
            {
                this.FillCommandNames(string.Empty, command, commandNames);
            }

            return commandNames;
        }

        private void FillCommandNames(string previousCommandName, Command command, ICollection<string> targetSource)
        {
            var commandName = !string.IsNullOrEmpty(previousCommandName) ?
                                    previousCommandName + " " + command.Name :
                                    command.Name;

            targetSource.Add(commandName);

            foreach (var subcommand in command.Subcommands)
            {
                this.FillCommandNames(commandName, subcommand, targetSource);
            }
        }

        private IEnumerable<Command> GetMappedCommands(IEnumerable<XElement> xCommands)
        {
            var commands = new List<Command>();

            if (xCommands != null)
            {
                foreach (var xCommand in xCommands)
                {
                    var command = new Command
                    {
                        Description = xCommand.Attribute("description").Value,
                        Name = xCommand.Attribute("name").Value,
                        Subcommands = this.GetMappedCommands(xCommand.Elements("Command"))
                    };
                    commands.Add(command);
                }
            }

            return commands;
        }
    }
}
