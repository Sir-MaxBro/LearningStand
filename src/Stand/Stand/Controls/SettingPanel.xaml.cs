using Stand.Domain.Extensions;
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
        private readonly char[] allowedCharacters = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
        private string _ipAddress;
        private int _port;
        private bool _isPortValid;
        private bool _isIPValid;

        public event PropertyChangedEventHandler PropertyChanged;
        public event RoutedEventHandler Connect;
        public event RoutedEventHandler Disconnect;

        public SettingPanel()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Port = 23;
            this.IPAddress = "10.203.0.3";
        }

        public bool CanConnect
        {
            get { return _isPortValid && _isIPValid; }
        }

        public bool CanDisconnect
        {
            get { return this.CanConnect; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged("IPAddress");
                }
            }
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
            _isPortValid = int.TryParse(text, out int result);
            RefreshConnectEnable();
        }

        private void OnIPChanged(object sender, TextChangedEventArgs e)
        {
            _isIPValid = false;
            var text = (sender as TextBox)?.Text;
            if (!string.IsNullOrEmpty(text))
            {
                var textWithoutAllowed = text?.Except(allowedCharacters);
                if (string.IsNullOrEmpty(textWithoutAllowed))
                {
                    _isIPValid = true;
                }
            }
            RefreshConnectEnable();
        }

        private void RefreshConnectEnable()
        {
            OnPropertyChanged("CanConnect");
            OnPropertyChanged("CanDisconnect");
        }
    }
}
