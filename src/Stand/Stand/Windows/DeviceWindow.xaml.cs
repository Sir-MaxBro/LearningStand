using Stand.Domain.Abstract;
using Stand.Domain.Exceptions;
using Stand.Domain.Infractructure.EventArgs;
using Stand.UI.Infrastructure.EventArgs;
using System;
using System.Windows;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        private Device _device;
        private string _ipAddress;
        private int _port;
        private bool _isConnected;
        public DeviceWindow(Device device)
        {
            this._device = device;
            this._device.AnswerReceived += AnswerReceiver;

            InitializeComponent();
            if (!String.IsNullOrEmpty(device.DeviceName))
            {
                this.Title = device.DeviceName;
            }

            this.DataContext = this;
            taskPanel.Compiler = device.Compiler;
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
            if (!_isConnected)
            {
                // destroy dependency
                bool connectedResult = _device.Connect(settingPanel.IPAddress, settingPanel.Port);

                if (!connectedResult)
                {
                    string message = "Не удалось установить подключение.";
                    MessageBox.Show(message, "Error");
                }
                _isConnected = connectedResult;
            }
        }

        private void Disconnect_ButtonClick(object sender, RoutedEventArgs e)
        {
            _device.Disconnect();
            _isConnected = false;
        }

        private void AnswerReceiver(object sender, ReceivedEventArgs args)
        {
            terminal.AddText(args.Answer);
        }
    }
}
