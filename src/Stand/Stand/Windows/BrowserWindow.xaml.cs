using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _currentUrl;

        public string CurrentUrl
        {
            get { return _currentUrl; }
            set
            {
                if (_currentUrl != value)
                {
                    _currentUrl = value;
                    OnPropertyChanged("CurrentUrl");
                }
            }
        }
        public BrowserWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var inputKey = (sender as TextBox).Text;
            if (inputKey != null)
            {
                LoadWebPage(inputKey);
            }
        }
        private void LoadWebPage(string url)
        {
            Browser.Navigate(url);
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnSubmit_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_currentUrl!=null)
            {
                Browser.Navigate(_currentUrl);
            }
          
        }
    }
}
