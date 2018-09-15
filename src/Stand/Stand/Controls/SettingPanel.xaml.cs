using System.Windows;
using System.Windows.Controls;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для SettingPanel.xaml
    /// </summary>
    public partial class SettingPanel : UserControl
    {
        private string _ipAddress;
        private int _port;
        public event RoutedEventHandler Connect;
        public event RoutedEventHandler Disconnect;
        public SettingPanel()
        {
            InitializeComponent();
            this.DataContext = this;
            _ipAddress = "10.203.0.3";
            _port = 23;
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set { this._ipAddress = value; }
        }

        public int Port
        {
            get { return _port; }
            set { this._port = value; }
        }

        private void Connect_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Connect != null)
            {
                Connect(sender, e);
            }
        }

        private void Disconnect_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Disconnect != null)
            {
                Disconnect(sender, e);
            }
        }
    }
}
