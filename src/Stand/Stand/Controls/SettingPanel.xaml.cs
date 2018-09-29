using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для SettingPanel.xaml
    /// </summary>
    public partial class SettingPanel : UserControl, INotifyPropertyChanged
    {
        private string _ipAddress = "10.203.0.3";
        private int _port;
        private bool _isPortValid;

        public event PropertyChangedEventHandler PropertyChanged;
        public event RoutedEventHandler Connect;
        public event RoutedEventHandler Disconnect;

        public SettingPanel()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Port = 23;
        }

        public bool CanConnect
        {
            get { return _isPortValid; }
        }

        public bool CanDisconnect
        {
            get { return this.CanConnect; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int Port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged("Port");
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(name));
            }
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

        protected virtual void OnPortChanged(object sender, TextChangedEventArgs e)
        {
            var text = (sender as TextBox)?.Text;
            int result;
            _isPortValid = int.TryParse(text, out result);
            RefreshConnectEnable();
        }

        private void RefreshConnectEnable()
        {
            OnPropertyChanged("CanConnect");
            OnPropertyChanged("CanDisconnect");
        }
    }
}
