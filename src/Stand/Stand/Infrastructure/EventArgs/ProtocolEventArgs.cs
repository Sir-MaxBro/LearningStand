using System.Windows;

namespace Stand.UI.Infrastructure.EventArgs
{
    public class ProtocolEventArgs : RoutedEventArgs
    {
        public string ProtocolName { get; set; }
    }
}
