using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace iMax.CommandParser
{
    public class Parser
    {
        private readonly XDocument _xdocument;
        public Parser(string source)
        {
            try
            {
                _xdocument = XDocument.Parse(source);
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }

        public ValidResult IsValid(string command)
        {
            var fullCommand = string.Empty;
            var mostSimilarCommand = string.Empty;
            bool isValid = false;
            var currentCommands = _xdocument.Elements("Commands").Elements("Command").ToList();
            string[] searchCommands = command
                .Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int maxLevel = searchCommands.Length;

            for (int i = 0; i < maxLevel; i++)
            {
                isValid = false;
                for (int j = 0; j < currentCommands.Count; j++)
                {
                    string currentCommand = currentCommands.ElementAt(j).Attribute("name").Value;
                    if (currentCommand.Equals(searchCommands[i]))
                    {
                        isValid = true;
                        fullCommand += searchCommands[i] + " ";
                        currentCommands = currentCommands.ElementAt(j).Elements("Command").ToList();
                        break;
                    }
                }

                if (!isValid)
                {
                    IEnumerable<string> similarCommands = currentCommands.Attributes("name")
                        .Select(x => x.Value).ToList();
                    mostSimilarCommand = GetMostSimilar(similarCommands, searchCommands[i]);
                    break;
                }
            }

            return new ValidResult
            {
                IsValid = isValid,
                MostSimilarCommand = fullCommand + " " + mostSimilarCommand
            };
        }

        private string GetMostSimilar(IEnumerable<string> commands, string currentCommand)
        {
            string mostSimilarCommand = string.Empty;
            double maxWeight = 0.0f;
            ISimilarity similarity = new Leven();
            foreach (var item in commands)
            {
                double currentWeight = similarity.GetSimilarity(item, currentCommand);
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
