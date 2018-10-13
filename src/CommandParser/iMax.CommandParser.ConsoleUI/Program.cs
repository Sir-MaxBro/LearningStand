using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace iMax.CommandParser.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlPath = @"E:\Projects\science\source\commands.xml";
            string command = "commandL1 commandL2 commandL3 commandL334";
            bool isValid = false;
            Console.WriteLine(command);

            Parser parser = new Parser(xmlPath);     
            ValidResult validResult = parser.IsValid(command);
            isValid = validResult.IsValid;
            if (!isValid)
            {
                Console.WriteLine("Command not Found");
                Console.WriteLine(validResult.MostSimilarCommand);
            }

            Console.WriteLine(isValid);

            Console.ReadKey();
        }
    }
}
