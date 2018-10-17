using Stand.Domain.Extensions;
using Stand.IoC.DependencyInjection;
using Stand.UI.Infrastructure.EventArgs;
using Stand.UI.Infrastructure.Events;
using System.Collections.Generic;
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
        private string _ipAddress;
        private int _port;
        private bool _isPortValid;
        private bool _isIPValid;
        private readonly Dictionary<string, int> _protocolNames = new Dictionary<string, int>
        {
            { IoCKeys.TelnetProtocolKey, 23 },
            { IoCKeys.SshProtocolKey, 22 },
        };

        public event RoutedEventHandler Connect;
        public event RoutedEventHandler Disconnect;
        public event ProtocolEventHandler ProtocolChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingPanel()
        {
            InitializeComponent();
            this.DataContext = this;
            this.IPAddress = "127.0.0.1";
        }

        public bool CanConnect
        {
            get { return _isPortValid && _isIPValid; }
        }

        public bool CanDisconnect
        {
            get { return this.CanConnect; }
        }

        public bool CanInputProtocol
        {
            get { return false; }
        }

        public IEnumerable<string> ProtocolNames
        {
            get { return _protocolNames.Keys; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    this.OnPropertyChanged("IPAddress");
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
                    this.OnPropertyChanged("Port");
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Connect_ButtonClick(object sender, RoutedEventArgs e)
        {
            var connect = this.Connect;
            if (connect != null)
            {
                connect(sender, e);
            }
        }

        private void Disconnect_ButtonClick(object sender, RoutedEventArgs e)
        {
            var disconnect = this.Disconnect;
            if (disconnect != null)
            {
                disconnect(sender, e);
            }
        }

        protected virtual void OnPortChanged(object sender, TextChangedEventArgs e)
        {
            var text = (sender as TextBox)?.Text;
            _isPortValid = int.TryParse(text, out int result);
            this.RefreshConnectEnable();
        }

        private void OnIPChanged(object sender, TextChangedEventArgs e)
        {
            var text = (sender as TextBox)?.Text;

            _isIPValid = false;
            (sender as TextBox).ToolTip = "Input there correct ip-address";
            if (!string.IsNullOrEmpty(text))
            {
                System.Net.IPAddress realIPAddress;
                if (System.Net.IPAddress.TryParse(text, out realIPAddress))
                {
                    (sender as TextBox).ToolTip = realIPAddress;
                    _isIPValid = true;
                }
            }
            this.RefreshConnectEnable();
        }

        private void RefreshConnectEnable()
        {
            this.OnPropertyChanged("CanConnect");
            this.OnPropertyChanged("CanDisconnect");
        }

        private void OnProtocolChanged(object sender, SelectionChangedEventArgs e)
        {
            var protocolName = (sender as ComboBox).SelectedValue.ToString();
            this.Port = _protocolNames[protocolName];
            this.OnPropertyChanged("CanInputProtocol");

            var protocolChanged = this.ProtocolChanged;
            if (protocolChanged != null)
            {
                protocolChanged(sender, new ProtocolEventArgs { ProtocolName = protocolName });
            }
        }
    }
}
