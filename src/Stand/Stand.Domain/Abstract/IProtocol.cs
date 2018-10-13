using Stand.General.Insrastructure.Params;
using System.Threading.Tasks;

namespace Stand.Domain.Abstract
{
    public interface IProtocol
    {
        Task<bool> ConnectAsync(ConnectionParams connectionParams);

        void Disconnect();

        Task<string> ExecuteCommandAsync(string command);
    }
}
