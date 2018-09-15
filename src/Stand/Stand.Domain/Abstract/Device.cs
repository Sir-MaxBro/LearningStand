using Stand.Domain.Exceptions;
using Stand.Domain.Infractructure.Events;
using System.Text;

namespace Stand.Domain.Abstract
{
    public abstract class Device
    {
        protected IProtocol _protocol;
        protected ICompiler _compiler;
        protected Device(IProtocol protocol, ICompiler compiler)
        {
            this._protocol = protocol;
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
            return _protocol.Connect(host, port);
        }

        public void Disconnect()
        {
            _protocol.Disconnect();
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
                //answer.Append(_invitation + "#");
                //SendAnswer(answer.ToString());
                throw new CommandNotFoundException(answer.ToString());
                //global::System.Windows.Forms.MessageBox.Show("Test");
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
                _protocol.AnswerReceived += value;
            }
            remove
            {
                _protocol.AnswerReceived -= value;
            }
        }
    }
}
