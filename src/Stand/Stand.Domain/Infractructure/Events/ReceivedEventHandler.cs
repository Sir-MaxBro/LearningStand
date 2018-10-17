using Stand.Domain.Infractructure.EventArgs;

namespace Stand.Domain.Infractructure.Events
{
    public delegate void ReceivedEventHandler(object sender, ReceivedEventArgs args);
}
