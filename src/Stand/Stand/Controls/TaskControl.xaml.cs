using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl, INotifyPropertyChanged
    {
        private readonly Brush DONE_COLOR = Brushes.Green;
        private readonly Brush NOT_DONE_COLOR = Brushes.Gray;
        private readonly Brush DEFAULT_COLOR = Brushes.Black;
        private readonly FontWeight CURRENT_FONT_WEIGHT = FontWeights.Bold;
        private readonly FontWeight DEFAULT_FONT_WEIGHT = FontWeights.Normal;
        private const string MARKER = "\u2713";

        private string _taskDescription;
        private string _rightTaskCommand;
        private bool _isDone;
        private bool _isCurrent;
        private Brush _textColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public TaskControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.markerLabel.Content = MARKER;
        }

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Description
        {
            get { return _taskDescription; }
            set
            {
                if (_taskDescription != value)
                {
                    _taskDescription = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }

        public string RightTaskCommand
        {
            get { return _rightTaskCommand; }
            set { _rightTaskCommand = value; }
        }

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                _isDone = value;
                if (_isDone)
                {
                    this.TextColor = DONE_COLOR;
                    taskTextBox.FontWeight = DEFAULT_FONT_WEIGHT;
                }
                else
                {
                    this.TextColor = NOT_DONE_COLOR;
                }
            }
        }

        public Brush TextColor
        {
            get { return _textColor; }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    this.OnPropertyChanged("TextColor");
                }
            }
        }

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                _isCurrent = value;
                if (_isCurrent)
                {
                    taskTextBox.FontWeight = CURRENT_FONT_WEIGHT;
                    this.TextColor = DEFAULT_COLOR;
                }
            }
        }
    }
}
