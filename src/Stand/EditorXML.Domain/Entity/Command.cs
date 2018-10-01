﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EditorXML.Domain.Entity
{
    [XmlRoot(ElementName = "command")]
    public class Command:ICloneable
    {
        [XmlAttribute("name")]
        private String _name;
        [XmlAttribute("description")]
        private string _definiton;
        private List<Command> _subCommands;

        public Command()
        {
            _subCommands = new List<Command>();
        }
      

        public List<Command> SubCommands
        {
            get { return _subCommands; }
            set { _subCommands = value; }
        }
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Definiton
        {
            get { return _definiton; }
            set { _definiton = value; }
        }
        public void AddCommand(Command subCommand)
        {
            _subCommands.Add(subCommand);
        }
        public void RemoveCommand(Command subCommand)
        {
            _subCommands.Remove(subCommand);
        }
        public override string ToString()
        {
            return _name;
        }



        public object Clone()
        {
            Command clone = new Command();
            clone.Name = _name;
            clone.Definiton =_definiton;
            clone.SubCommands =new List<Command>(_subCommands);
            return clone;
        }
    }
}
