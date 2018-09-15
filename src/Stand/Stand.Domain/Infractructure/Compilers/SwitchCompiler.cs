using iMax.CommandParser;
using Stand.Domain.Abstract;

namespace Stand.Domain.Infractructure.Compilers
{
    public class SwitchCompiler : ICompiler
    {
        private static Parser _parser;
        private string _resource;
        public SwitchCompiler()
        { }

        public string Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }

        public ValidResult IsValid(string command)
        {
            command = command.Remove(0, command.IndexOf('#') + 1);
            if (_parser == null)
            {
                _parser = new Parser(_resource);
            }
            var validResult = _parser.IsValid(command);
            return validResult;
        }
    }
}
