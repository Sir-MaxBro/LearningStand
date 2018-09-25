using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
	public partial class ExceptionWindow : Window, INotifyPropertyChanged
    {
        private const int SECONDS_IN_MINUTES = 60;
        private const string TIME_FORMAT = "{0}:{1:D2}";

        public event PropertyChangedEventHandler PropertyChanged;
        private event Action<DispatcherTimer> _timeOut;

        private DispatcherTimer _timer;
        private string _timeLeft;
        private int _timeLeftTime;

        public ExceptionWindow(string errorMessage, int seconds)
        {
            this.ErrorMessage = errorMessage;
            _timeLeftTime = seconds;

            TimeLeft = this.GetTimeString(_timeLeftTime);

            InitializeComponent();
            this.DataContext = this;
        }

        public string ErrorMessage { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timeOut += TimeOuts;
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void TimeOuts(DispatcherTimer timer)
        {
            timer.Stop();
            this.Close();
        }

        public string TimeLeft
        {
            get { return _timeLeft; }
            set
            {
                if (_timeLeft != value)
                {
                    _timeLeft = value;
                    this.OnPropertyChanged("TimeLeft");

                }
                if (_timeLeftTime == 0)
                {
                    if (_timeOut != null)
                    {
                        _timeOut(_timer);
                    }
                }
            }
        }

        public void OnPropertyChanged(string prop = "")
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _timeLeftTime--;
            this.TimeLeft = this.GetTimeString(_timeLeftTime);
        }

        private string GetTimeString(int seconds)
        {
            return string.Format(TIME_FORMAT, seconds / SECONDS_IN_MINUTES, seconds % SECONDS_IN_MINUTES);
        }
    }
}
