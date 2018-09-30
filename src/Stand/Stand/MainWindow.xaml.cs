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
        public MainWindow()
        {
            this.InitializeComponent();
            this.BindingMenu();
        }

        private void BindingMenu()
        {
            var menuItems = MenuData.GetMenuItems();
            if (!menuItems.Any())
            {
                labsMenuItem.IsEnabled = false;
            }
            else
            {
                labsMenuItem.ItemsSource = menuItems;
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
                var protocol = protocolWindow.Protocol;
                string deviceName = (sender as Control).Tag.ToString();

                // get device
                var device = IoC.GetDevice(deviceName, protocol);
                device.DeviceName = deviceName.Substring(0, 1).ToUpper() + deviceName.Substring(1, deviceName.Length - 1);

                // open deviceWindow
                DeviceWindow deviceWindow = new DeviceWindow(device);
                // deviceWindow.Owner = this;
                deviceWindow.Top = this.Top;
                deviceWindow.Left = this.Left;
                deviceWindow.Show();
            }
        }

        protected virtual void OnSettingsOpen(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        protected virtual void OnHelpOpen(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }
    }
}
