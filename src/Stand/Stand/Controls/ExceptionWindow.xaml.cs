using System;
using System.ComponentModel;
using System.Windows;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
	public partial class ExceptionWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private event Action<System.Windows.Threading.DispatcherTimer> timeOut;

        public string ExceptionText { get; set; }
        private string _timeLeft;
        private int _timeLeftTime;
        private System.Windows.Threading.DispatcherTimer timer;

        public ExceptionWindow(string text, int seconds)
        {
            ExceptionText = text;
            _timeLeftTime = seconds;
            TimeLeft = String.Format("{0}:{1:D2}", _timeLeftTime / 60, _timeLeftTime % 60);
            InitializeComponent();
            this.DataContext = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timeOut += TimeOuts;
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        private void TimeOuts(System.Windows.Threading.DispatcherTimer timer)
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
                    OnPropertyChanged("TimeLeft");

                }
                if (_timeLeftTime == 0)
                {
                    if (timeOut != null)
                    {
                        timeOut(timer);
                    }
                }
            }
        }
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        private void timerTick(object sender, EventArgs e)
        {
            _timeLeftTime--;
            TimeLeft = String.Format("{0}:{1:D2}", _timeLeftTime / 60, _timeLeftTime % 60);
        }
    }
}
