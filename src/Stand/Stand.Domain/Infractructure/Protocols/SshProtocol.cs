using Renci.SshNet;
using Renci.SshNet.Common;
using Stand.Domain.Abstract;
using Stand.General.Insrastructure.Params;
using System.Threading.Tasks;

namespace Stand.Domain.Infractructure.Protocols
{
    public class SshProtocol : IProtocol
    {
        private SshClient _sshClient;

        public async Task<bool> ConnectAsync(ConnectionParams connectionParams)
        {
            var host = connectionParams.Host;
            var port = connectionParams.Port;
            var username = connectionParams.Username;
            var password = connectionParams.Password;

            try
            {
                _sshClient = new SshClient(host, port, username, password);
                await Task.Run(() => _sshClient.Connect());
            }
            catch (SshOperationTimeoutException ex)
            {
                return false;
            }

            return true;
        }

        public void Disconnect()
        {
            if (_sshClient != null)
            {
                _sshClient.Disconnect();
            }
        }

        public async Task<string> ExecuteCommandAsync(string command)
        {
            var answer = string.Empty;
            if (_sshClient != null)
            {
                var sshCommand = _sshClient.CreateCommand(command);
                answer = await Task.Run(() => sshCommand.Execute());
            }
            return answer;
        }
    }
}
