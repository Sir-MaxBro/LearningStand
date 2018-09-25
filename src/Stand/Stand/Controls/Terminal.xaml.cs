using Stand.Domain.Exceptions;
using Stand.UI.Exceptions;
using Stand.UI.Infrastructure.EventArgs;
using Stand.UI.Infrastructure.Events;
using Stand.UI.Windows;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для Terminal.xaml
    /// </summary>
    public partial class Terminal : UserControl, INotifyPropertyChanged
    {
        private const string NEW_LINE = "\n";

        public event CommandEventHandler SendingCommandToExecute;
        public event PropertyChangedEventHandler PropertyChanged;
        private TextBox _terminal;
        private string _text;
        private int _caretIndex;

        public Terminal()
        {
            InitializeComponent();
            this.DataContext = this;
            _terminal = terminal;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Terminal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.SetCommandToExecute();
            }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                this.SetCaretToEnd(e, (x, y) => x >= y);
            }
        }

        private void Terminal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            this.SetCaretToEnd(e, (x, y) => x > y);
        }

        private void SetCaretToEnd(RoutedEventArgs e, Func<int, int, bool> checkCaretIndex)
        {
            if (checkCaretIndex(_caretIndex, _terminal.CaretIndex))
            {
                e.Handled = true;
                _terminal.Focus();
                _terminal.CaretIndex = _terminal.Text.Length;
                _terminal.ScrollToEnd();
            }
        }

        private void ExecuteCommand(string currentCommand)
        {
            var sendingCommandToExecute = this.SendingCommandToExecute;
            if (sendingCommandToExecute != null)
            {
                sendingCommandToExecute(this, new CommandEventArgs { Command = currentCommand });
            }
        }

        private void SetCommandToExecute()
        {
            try
            {
                var command = _terminal.GetLineText(_terminal.LineCount - 1);
                this.ExecuteCommand(command);
                _terminal.CaretIndex = _terminal.Text.Length;
                _caretIndex = _terminal.CaretIndex;
            }
            catch (CommandNotFoundException ex)
            {
                ExceptionWindow window = new ExceptionWindow(ex.Message, 25);
                window.ShowDialog();
            }
            catch (CommandNotMatchAssignment ex)
            {
                ExceptionWindow window = new ExceptionWindow(ex.Message, 25);
                window.ShowDialog();
            }
            catch (EmptyTaskCommandsException ex)
            {
                ExceptionWindow window = new ExceptionWindow(ex.Message, 25);
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionWindow window = new ExceptionWindow(ex.Message, 25);
                window.ShowDialog();
            }
        }

        private void CommandBinding_CanExecuteCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = false;
        }

        private void CommandBinding_CanExecuteCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void CommandBinding_CanExecutePaste(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_caretIndex > _terminal.CaretIndex)
            {
                e.CanExecute = false;
                e.Handled = true;
            }
            else
            {
                e.CanExecute = true;
                e.Handled = false;
            }
        }

        public new void AddText(string text)
        {
            _terminal.Text += text;
            _terminal.ScrollToEnd();
        }
    }
}
