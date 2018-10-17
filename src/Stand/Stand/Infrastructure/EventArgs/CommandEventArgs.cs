using System.Windows;

namespace Stand.UI.Infrastructure.EventArgs
{
    public class CommandEventArgs : RoutedEventArgs
    {
        public string Command { get; set; }
    }
}
