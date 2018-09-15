using Stand.Domain.Abstract;
using Stand.UI.Infrastructure;
using Stand.UI.Menu;
using Stand.UI.Windows;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Stand.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IProtocol _protocol;
        private Device _device;
        public MainWindow()
        {
            InitializeComponent();
            BindingMenu();
        }

        private void BindingMenu()
        {
            if (MenuData.Items.Count() == 0)
            {
                labsMenuItem.IsEnabled = false;
            }
            else
            {
                labsMenuItem.ItemsSource = MenuData.Items;
            }
        }

        private void OnClick_Menu(object sender, RoutedEventArgs e)
        {
            var menuItem = e.OriginalSource as MenuItem;
            MenuData.OpenFile(menuItem.DataContext.ToString());
        }

        private void SwitchOpen_ButtonClick(object sender, RoutedEventArgs e)
        {
            ProtocolWindow protocolWindow = new ProtocolWindow();
            protocolWindow.Owner = this;

            if (protocolWindow.ShowDialog().Value)
            {
                // get protocol
                _protocol = protocolWindow.Protocol;
                string deviceName = (sender as Control).Tag.ToString();

                // get device
                _device = DependencyContainer.GetDevice(deviceName, _protocol);
                _device.DeviceName = deviceName.Substring(0, 1).ToUpper() + deviceName.Substring(1, deviceName.Length - 1);
                // open deviceWindow
                DeviceWindow deviceWindow = new DeviceWindow(_device);
                // deviceWindow.Owner = this;
                deviceWindow.Top = this.Top;
                deviceWindow.Left = this.Left;
                deviceWindow.Show();
            }
        }
    }
}
