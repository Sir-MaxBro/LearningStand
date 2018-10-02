using EditorXML.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EditorXML.Domain.Abstract
{
   public interface ICommandDAO
    {
       List<Command> LoadCommands(string path);

       void Save(ICollection<Command> commands, String path);

    }
}
