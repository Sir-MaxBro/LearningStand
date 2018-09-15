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
        private string _taskDescription;
        private bool _isDone;
        private string _taskCommand;
        private readonly Brush IS_DONE_COLOR = Brushes.Green;
        private readonly Brush IS_NOT_DONE_COLOR = Brushes.Gray;
        private readonly Brush CURRENT_COLOR = Brushes.Black;
        private Brush _textColor;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isCurrent;
        public TaskControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Description
        {
            get { return _taskDescription; }
            set
            {
                if (_taskDescription != value)
                {
                    _taskDescription = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string TaskCommand
        {
            get { return _taskCommand; }
            set { _taskCommand = value; }
        }

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                _isDone = value;
                if (_isDone)
                {
                    TextColor = IS_DONE_COLOR;
                    taskTxt.FontWeight = FontWeights.Normal;
                }
                else
                {
                    TextColor = IS_NOT_DONE_COLOR;
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
                    OnPropertyChanged("TextColor");
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
                    taskTxt.FontWeight = FontWeights.Bold;
                    TextColor = CURRENT_COLOR;
                }
            }
        }
    }
}
