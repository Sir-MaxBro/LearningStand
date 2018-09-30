using Stand.General.Insrastructure.Settings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        private readonly SettingsService _settingsService;
        private General.Entities.Settings _settings;

        public event PropertyChangedEventHandler PropertyChanged;
        private IDictionary<object, bool> _errors;

        public SettingsWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _settingsService = SettingsService.GetInstance();
            _errors = new Dictionary<object, bool>();

            this.RefreshSettings();
        }

        public General.Entities.Settings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                this.OnPropertyChanged("Settings");
            }
        }

        public bool CanSave
        {
            get { return !_errors.Values.Any(error => error == false); }
        }

        protected void OnPropertyChanged(string name)
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void RefreshSettings()
        {
            if (_settingsService != null)
            {
                this.Settings = _settingsService.GetSettings();
            }
        }

        protected virtual void OnSaveSettings(object sender, RoutedEventArgs e)
        {
            if (this.Settings != null)
            {
                _settingsService.SetSettings(this.Settings);
                this.RefreshSettings();
            }
            this.Close();
        }

        protected virtual void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnTextNumericValidation(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            var text = textBox.Text;
            var isValid = !string.IsNullOrEmpty(text) && int.TryParse(text, out int result);
            this.CheckValid(textBox, isValid);
        }

        private void OnTextValidation(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            var text = textBox.Text;
            var isValid = !string.IsNullOrEmpty(text);
            this.CheckValid(textBox, isValid);
        }

        protected virtual void CheckValid(object obj, bool isValid)
        {
            if (!_errors.ContainsKey(obj))
            {
                _errors.Add(obj, false);
            }
            _errors[obj] = isValid;
            this.OnPropertyChanged("CanSave");
        }
    }
}
