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
            if (e.Key == Key.Enter)
            {
                LoadWebPage(inputKey);
            }
        }
        private void LoadWebPage(string url)
        {
            Browser.Navigate(url);
        }

        private void OnSubmit_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_currentUrl != null)
            {

                string prefix = _currentUrl.StartsWith("http://") || _currentUrl.StartsWith("https://") ? "" : "http://";
                Browser.Navigate(prefix + _currentUrl);
            }

        }
        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((Browser != null) && (Browser.CanGoBack));
            (e.OriginalSource as Button).Visibility = e.CanExecute ? Visibility.Visible : Visibility.Hidden;
        }
        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Browser.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((Browser != null) && (Browser.CanGoForward));

            (e.OriginalSource as Button).Visibility = e.CanExecute ? Visibility.Visible : Visibility.Hidden;
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Browser.GoForward();
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            _currentUrl = e.Uri.AbsoluteUri;
            OnPropertyChanged("CurrentUrl");
        }
    }
}
