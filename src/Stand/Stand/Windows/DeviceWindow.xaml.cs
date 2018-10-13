using Stand.Domain.Abstract;
using Stand.Domain.Exceptions;
using Stand.General.Insrastructure.Params;
using Stand.IoC.DependencyInjection;
using Stand.UI.Exceptions;
using Stand.UI.Infrastructure.EventArgs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Device _device;
        private bool _isConnected;

        public DeviceWindow(Device device)
        {
            _device = device;

            this.InitializeComponent();

            if (!string.IsNullOrEmpty(device.DeviceName))
            {
                this.Title = device.DeviceName;
            }

            taskPanel.Compiler = device.Compiler;
            this.IsConnected = false;
            this.DataContext = this;
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged("IsConnected");
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

        private async Task Terminal_SendingCommandToExecute(object sender, CommandEventArgs e)
        {
            var answer = string.Empty;
            try
            {
                string[] commandSplit = e.Command.Split(':', '#');
                if (!commandSplit[0].ToLower().Contains("password"))
                {
                    taskPanel.CheckTask(e.Command);
                    answer = await _device.ExecuteCommandAsync(e.Command);
                }
                else
                {
                    answer = await _device.TryPassword(commandSplit[1]);
                }
            }
            catch (CommandNotFoundException ex)
            {
                throw ex;
            }
            catch (EmptyTaskCommandsException ex)
            {
                throw ex;
            }
            catch (CommandNotMatchAssignment ex)
            {
                throw ex;
            }

            if (!string.IsNullOrEmpty(answer))
            {
                terminal.AddText(answer);
            }
        }

        private async void Connect_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!this.IsConnected)
            {
                var connectionParams = new ConnectionParams
                {
                    Host = settingPanel.IPAddress,
                    Port = settingPanel.Port,
                    Username = "myname",
                    Password = "mypassword",
                };

                bool isConnectedSuccess = await _device.ConnectAsync(connectionParams);

                if (!isConnectedSuccess)
                {
                    string errorMessage = "Не удалось установить подключение.";
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.IsConnected = isConnectedSuccess;
            }
        }

        private void Disconnect_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_device != null)
            {
                _device.Disconnect();
            }
            this.IsConnected = false;
        }

        private void OnProtocolChanged(object sender, ProtocolEventArgs e)
        {
            var protocolName = e.ProtocolName;
            var protocol = IoCContainer.GetProtocol(protocolName);
            _device.Protocol = protocol;
        }
    }
}
