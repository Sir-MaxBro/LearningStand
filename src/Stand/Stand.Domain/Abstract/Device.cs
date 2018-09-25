using Stand.Domain.Abstract.Contracts;
using Stand.Domain.Exceptions;
using Stand.Domain.Infractructure.Events;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Stand.Domain.Abstract
{
    [ContractClass(typeof(DeviceContract))]
    public abstract class Device
    {
        private ReceivedEventHandler _receivedEvent;

        protected IProtocol _protocol;
        protected ICompiler _compiler;

        protected Device(IProtocol protocol, ICompiler compiler)
        {
            _protocol = protocol;
            _compiler = compiler;
        }

        public string DeviceName { get; set; }

        public string ProtocolName { get; set; }

        public ICompiler Compiler
        {
            get { return _compiler; }
        }

        public bool Connect(string host, int port)
        {
            if (string.IsNullOrEmpty(host))
            {
                return false;
            }

            return _protocol.Connect(host, port);
        }

        public void Disconnect()
        {
            if (_protocol != null)
            {
                _protocol.Disconnect();
            }
        }

        public void ExecuteCommand(string command)
        {
            var validResult = _compiler.IsValid(command);
            if (validResult.IsValid)
            {
                _protocol.ExecuteCommand(command);
            }
            else
            {
                StringBuilder answer = new StringBuilder("Команда не найдена\n");
                answer.AppendLine("Может вы имели ввиду: " + validResult.MostSimilarCommand);
                throw new CommandNotFoundException(answer.ToString());
            }
        }

        public void TryPassword(string password)
        {
            _protocol.TryPassword(password.Trim());
        }

        public event ReceivedEventHandler AnswerReceived
        {
            add
            {
                lock (this)
                {
                    if (_receivedEvent == null
                        || !_receivedEvent.GetInvocationList().Any(del => del.Method == value.Method))
                    {
                        _receivedEvent += value;
                    }
                }
            }
            remove
            {
                lock (this)
                {
                    _receivedEvent -= value;
                }
            }
        }
    }
}
