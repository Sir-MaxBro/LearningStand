using PrimS.Telnet;
using Stand.Domain.Abstract;
using Stand.Domain.Infractructure.EventArgs;
using Stand.Domain.Infractructure.Events;
using Stand.General.Insrastructure.Settings;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Stand.Domain.Infractructure.Protocols
{
    public class TelnetProtocol : IProtocol
    {
        private static Client _telnetClient;

        private readonly TimeSpan _timeOutWaitingAnswer;
        private readonly TimeSpan TIME_OUT_TICK = new TimeSpan(1000);

        public event ReceivedEventHandler AnswerReceived;

        public TelnetProtocol()
        {
            var settingsService = SettingsService.GetInstance();
            var connectionTimeOut = settingsService.GetSettings().ConnectionTimeOut;
            _timeOutWaitingAnswer = new TimeSpan(hours: 0, minutes: 0, seconds: connectionTimeOut);
        }

        public bool Connect(string host, int port)
        {
            try
            {
                _telnetClient = new Client(host, port, CancellationToken.None);
            }
            catch (SocketException ex)
            {
                return false;
            }

            this.WaitForAnswer();

            return _telnetClient.IsConnected;
        }

        public void Disconnect()
        {
            if (_telnetClient != null)
            {
                _telnetClient.Dispose();
                this.SendAnswer("\nСоединение разорвано.\n");
            }
        }

        public void ExecuteCommand(string command)
        {
            string[] commandSplit = command.Split(':', '#');

            string currentCommand = commandSplit.ElementAtOrDefault(1);

            if (string.IsNullOrEmpty(currentCommand))
            {
                return;
            }

            this.SendCommandToDevice(currentCommand.Trim());
        }

        private void SendCommandToDevice(string command)
        {
            if (_telnetClient != null && _telnetClient.IsConnected)
            {
                var bytes = Encoding.Default.GetBytes(command);
                command = Encoding.ASCII.GetString(bytes);

                _telnetClient.WriteLine(command);
                this.WaitForAnswer(command);
            }
        }

        private void WaitForAnswer(string command = "")
        {
            Thread answerThread = new Thread((objCommand) => this.GetAnswer((string)objCommand));
            answerThread.Start(command);
            if (!answerThread.Join(_timeOutWaitingAnswer))
            {
                answerThread.Abort();
                throw new TimeoutException("Timeout error.");
            }
        }

        private async void GetAnswer(string command = "")
        {
            string answer = string.Empty;
            if (_telnetClient != null)
            {
                do
                {
                    answer += await _telnetClient.ReadAsync(TIME_OUT_TICK);
                } while (string.IsNullOrEmpty(answer) || answer == command);
            }
            answer = answer.TrimStart(command.ToCharArray());
            this.SendAnswer(answer);
        }

        public void SendAnswer(string message)
        {
            var answerReceived = this.AnswerReceived;
            if (answerReceived != null)
            {
                var receivedEventArgs = new ReceivedEventArgs { Answer = message };
                answerReceived(this, receivedEventArgs);
            }
        }
    }
}
