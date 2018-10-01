using EditorXML.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EditorXML.Domain.Abstract
{
   public interface ICommandService
    {
       List<Command> ReadCommandsFromFile(string path);

       void Save(List<Command> commands, String path);

    }
}
