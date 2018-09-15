using Stand.Domain.Infractructure.Events;

namespace Stand.Domain.Abstract
{
    public interface IProtocol
    {
        event ReceivedEventHandler AnswerReceived;

        bool Connect(string host, int port);

        void Disconnect();

        void ExecuteCommand(string command);

        void TryPassword(string password);

        void SendAnswer(string message);
    }
}
