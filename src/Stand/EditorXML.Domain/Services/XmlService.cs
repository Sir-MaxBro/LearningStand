using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EditorXML.Domain.Service
{
   public class XmlService : IXmlService
    {
        public XDocument ReadFile(string path)
        {
            XDocument document;

            try
            {
                document = XDocument.Load(path);
            }
            catch (IOException ex)
            {
                throw ex;
            }

            return document;
        }

        public XDocument GetDefaultDocument()
        {
            XDocument document = new XDocument();
            XElement node = GetDefaultElement();

            node.Name = "commands";
            document.Add(node);

            return document;
        }

        public XElement GetDefaultElement()
        {
            XElement node = new XElement("command");

            node.Add(new XAttribute("name", ""));

            return node;
        }
    }
}
