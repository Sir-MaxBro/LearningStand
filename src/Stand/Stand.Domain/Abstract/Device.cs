using Stand.Domain.Abstract.Contracts;
using Stand.Domain.Exceptions;
using Stand.General.Insrastructure.Params;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace Stand.Domain.Abstract
{
    [ContractClass(typeof(DeviceContract))]
    public abstract class Device
    {
        protected IProtocol _protocol;
        protected ICompiler _compiler;

        protected Device(ICompiler compiler)
        {
            _compiler = compiler;
        }

        public string DeviceName { get; set; }

        public string ProtocolName { get; set; }

        public IProtocol Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        public ICompiler Compiler
        {
            get { return _compiler; }
        }

        public async Task<bool> ConnectAsync(ConnectionParams connectionParams)
        {
            if (connectionParams == null)
            {
                return false;
            }

            return _protocol != null ? await _protocol.ConnectAsync(connectionParams) : false;
        }

        public void Disconnect()
        {
            if (_protocol != null)
            {
                _protocol.Disconnect();
            }
        }

        public async Task<string> ExecuteCommandAsync(string command)
        {
            string answer = string.Empty;
            var validResult = _compiler.IsValid(command);
            if (validResult.IsValid)
            {
                answer = await _protocol.ExecuteCommandAsync(command);
            }
            else
            {
                StringBuilder exceptionAnswer = new StringBuilder("Команда не найдена\n");
                exceptionAnswer.AppendLine("Может вы имели ввиду: '" + validResult.MostSimilarCommand + "'");
                throw new CommandNotFoundException(exceptionAnswer.ToString());
            }

            return answer;
        }

        public async Task<string> TryPassword(string password)
        {
            return await _protocol.ExecuteCommandAsync(password.Trim());
        }
    }
}
