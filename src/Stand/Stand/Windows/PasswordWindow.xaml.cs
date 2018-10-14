using System.Windows;
using System.Windows.Controls;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Login { get; set; }

        public string Password { get; set; }

        protected virtual void OnSubmit_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        protected virtual void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (sender as PasswordBox);
            this.Password = passwordBox.Password;
        }
    }
}
