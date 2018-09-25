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
        private readonly Brush IS_DONE_COLOR = Brushes.Green;
        private readonly Brush IS_NOT_DONE_COLOR = Brushes.Gray;
        private readonly Brush CURRENT_COLOR = Brushes.Black;

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
                    this.TextColor = IS_DONE_COLOR;
                    taskTextBox.FontWeight = FontWeights.Normal;
                }
                else
                {
                    this.TextColor = IS_NOT_DONE_COLOR;
                }
            }
        }

        protected Brush TextColor
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
                    taskTextBox.FontWeight = FontWeights.Bold;
                    this.TextColor = CURRENT_COLOR;
                }
            }
        }
    }
}
