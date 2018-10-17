using Stand.Domain.Abstract;
using System;
using System.IO;

namespace Stand.Domain.Infractructure.Devices
{
    public class SwitchDevice : Device
    {
        public SwitchDevice(ICompiler compiler)
            : base(compiler)
        {
            string source = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\command_parse\\TelnetSwitchCommands.xml");
            compiler.Resource = source;
        }
    }
}
