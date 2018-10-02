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

namespace EditorXML.Domain.DAO
{
    public class XmlCommandDAO : ICommandDAO
    {
        private XmlSerializer serializer;
        public XmlCommandDAO()
        {
            serializer = new XmlSerializer(typeof(List<Command>), new XmlRootAttribute("commands"));
        }

        public List<Command> LoadCommands(string path)
        {
            List<Command> list = new List<Command>();

            using (FileStream stream = File.OpenRead(path))
            {
                list = (List<Command>)serializer.Deserialize(stream);
            }
            return list;
        }

        public void Save(ICollection<Command> commands, String path)
        {
            XmlSerializerNamespaces serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add("", "");

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, commands, serializerNamespaces);
            }
        }
    }
}
