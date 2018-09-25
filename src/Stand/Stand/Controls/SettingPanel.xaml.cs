using System.Windows;
using System.Windows.Controls;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для SettingPanel.xaml
    /// </summary>
    public partial class SettingPanel : UserControl
    {
        private string _ipAddress = "10.203.0.3";
        private int _port = 23;

        public event RoutedEventHandler Connect;
        public event RoutedEventHandler Disconnect;

        public SettingPanel()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private void Connect_ButtonClick(object sender, RoutedEventArgs e)
        {
            var connect = Connect;
            if (connect != null)
            {
                connect(sender, e);
            }
        }

        private void Disconnect_ButtonClick(object sender, RoutedEventArgs e)
        {
            var disconnect = Disconnect;
            if (disconnect != null)
            {
                disconnect(sender, e);
            }
        }
    }
}
