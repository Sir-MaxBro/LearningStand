using Stand.Domain.Abstract;
using Stand.UI.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProtocolWindow.xaml
    /// </summary>
    public partial class ProtocolWindow : Window
    {
        private IProtocol _protocol;
        public ProtocolWindow()
        {
            this.InitializeComponent();
        }

        public IProtocol Protocol
        {
            get
            {
                return _protocol;
            }
        }

        private void DialogResult_ButtonClick(object sender, RoutedEventArgs e)
        {
            // get protocol name from selected radioButton
            string protocolName = protocolsStackPanel.Children
                .OfType<RadioButton>()
                .FirstOrDefault(x => (bool)x.IsChecked)
                .Tag.ToString();
            _protocol = IoC.GetProtocol(protocolName);
            this.CloseWindow();
        }

        private void CloseWindow()
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
