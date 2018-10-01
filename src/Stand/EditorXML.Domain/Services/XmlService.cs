using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EditorXML.Domain.Abstract;
using EditorXML.Domain.Entity;
using System.Xml;
using System.Xml.Serialization;

namespace EditorXML.Domain.Service
{
    public class CommandService : ICommandService
    {

        public List<Command> ReadCommandsFromFile(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Command>));
            List<Command> list = new List<Command>();

            using (FileStream stream = File.OpenRead(path))
            {
                list = (List<Command>)serializer.Deserialize(stream);
            }
            return list;
        }

        public void Save(List<Command> commands, String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Command>));

            using (FileStream stream = new FileStream(path,FileMode.Create,FileAccess.Write))
            {
                serializer.Serialize(stream, commands);
            }
        }
    }
}
