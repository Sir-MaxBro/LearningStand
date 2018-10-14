using PrimS.Telnet;
using Stand.Domain.Abstract;
using Stand.General.Insrastructure.Params;
using Stand.General.Insrastructure.Settings;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Stand.Domain.Infractructure.Protocols
{
    public class TelnetProtocol : IProtocol
    {
        private const string DEFAULT_PASSWORD = "default_password";
        private readonly TimeSpan _timeOutWaitingAnswer;
        private readonly TimeSpan TIME_OUT_TICK = new TimeSpan(1000);

        private Client _telnetClient;

        public TelnetProtocol()
        {
            var settingsService = SettingsService.GetInstance();
            var connectionTimeOut = settingsService.GetSettings().ConnectionTimeOut;
            _timeOutWaitingAnswer = new TimeSpan(hours: 0, minutes: 0, seconds: connectionTimeOut);
        }

        public async Task<bool> ConnectAsync(ConnectionParams connectionParams)
        {
            var sampleCommand = "ping";
            try
            {
                _telnetClient = new Client(connectionParams.Host, connectionParams.Port, CancellationToken.None);
                await _telnetClient.WriteLine(sampleCommand);
                var answer = await this.GetAnswer(sampleCommand);
                var regex = new Regex("Password:");
                if (regex.Match(answer).Success)
                {
                    await this.SendCommandToDevice(connectionParams.Password ?? DEFAULT_PASSWORD);
                }
            }
            catch (SocketException ex)
            {
                return false;
            }

            return _telnetClient.IsConnected;
        }

        public void Disconnect()
        {
            if (_telnetClient != null)
            {
                _telnetClient.Dispose();
            }
        }

        public async Task<string> ExecuteCommandAsync(string command)
        {
            string[] commandSplit = command.Split(':', '#');

            string currentCommand = commandSplit.ElementAtOrDefault(1);

            if (string.IsNullOrEmpty(currentCommand))
            {
                return string.Empty;
            }

            return await this.SendCommandToDevice(currentCommand.Trim());
        }

        private async Task<string> SendCommandToDevice(string command)
        {
            var answer = string.Empty;
            if (_telnetClient != null && _telnetClient.IsConnected)
            {
                var bytes = Encoding.Default.GetBytes(command);
                command = Encoding.ASCII.GetString(bytes);

                await _telnetClient.WriteLine(command);
                answer = await this.GetAnswer(command);
            }
            return answer;
        }

        private async Task<string> GetAnswer(string command = "")
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
            return answer;
        }
    }
}
