using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace iMax.CommandParser
{
    public class Parser
    {
        private string _fullCommand;
        private string _mostSimilarCommand;
        private readonly XDocument _xdocument;
        public Parser(string source)
        {
            try
            {
                _xdocument = XDocument.Parse(source);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }
        }

        public ValidResult IsValid(string command)
        {
            _fullCommand = string.Empty;
            _mostSimilarCommand = string.Empty;
            bool isValid = false;
            var currentCommands = from xnode in _xdocument.Elements("Commands").Elements("Command")
                                  select xnode;
            string[] searchCommands = command
                .Split(new string[] { " " }, 
                StringSplitOptions.RemoveEmptyEntries);
            int maxLevel = searchCommands.Length;

            for (int i = 0; i < maxLevel; i++)
            {
                isValid = false;
                for (int j = 0; j < currentCommands.Count(); j++)
                {
                    string currentCommand = currentCommands.ElementAt(j).Attribute("name").Value;
                    if (currentCommand.Equals(searchCommands[i]))
                    {
                        isValid = true;
                        _fullCommand += searchCommands[i] + " ";
                        currentCommands = from xnode in currentCommands.ElementAt(j).Elements("Command")
                                          select xnode;
                        break;
                    }
                }

                if (!isValid)
                {
                    IEnumerable<string> similarCommands = currentCommands.Attributes("name")
                        .Select(x => x.Value);
                    _mostSimilarCommand = GetMostSimilar(similarCommands, searchCommands[i]);
                    break;
                }
            }

            return new ValidResult
            {
                IsValid = isValid,
                MostSimilarCommand = _fullCommand + " " + _mostSimilarCommand
            };
        }

        private string GetMostSimilar(IEnumerable<string> commands, string currentCommand)
        {
            string mostSimilarCommand = string.Empty;
            float maxWeight = 0.0f;
            ISimilarity similarity = new Leven();
            foreach (var item in commands)
            {
                float currentWeight = similarity.GetSimilarity(item, currentCommand);
                if (currentWeight > maxWeight)
                {
                    maxWeight = currentWeight;
                    mostSimilarCommand = item;
                }
            }
            return mostSimilarCommand;
        }
    }
}
