using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EditorXML.Domain.Abstract
{
   public interface IXmlService
    {
        XDocument ReadFile(string path);
        XDocument GetDefaultDocument();
        XElement GetDefaultElement();

    }
}
