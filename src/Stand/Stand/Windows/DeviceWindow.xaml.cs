using Stand.Domain.Abstract;
using Stand.Domain.Exceptions;
using Stand.Domain.Infractructure.EventArgs;
using Stand.UI.Infrastructure.EventArgs;
using System;
using System.ComponentModel;
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
        private string _ipAddress;
        private int _port;
        private bool _isConnected;

        public DeviceWindow(Device device)
        {
            _device = device;
            _device.AnswerReceived += this.AnswerReceiver;

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

        private void Terminal_SendingCommandToExecute(object sender, CommandEventArgs e)
        {
            try
            {
                string[] commandSplit = e.Command.Split(':', '#');
                if (!commandSplit[0].ToLower().Contains("password"))
                {
                    taskPanel.CheckTask(e.Command);
                    _device.ExecuteCommand(e.Command);
                }
                else
                {
                    _device.TryPassword(commandSplit[1]);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Connect_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!this.IsConnected)
            {
                bool isConnectedSuccess = _device.Connect(settingPanel.IPAddress, settingPanel.Port);

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

        private void AnswerReceiver(object sender, ReceivedEventArgs args)
        {
            terminal.AddText(args.Answer);
        }
    }
}
