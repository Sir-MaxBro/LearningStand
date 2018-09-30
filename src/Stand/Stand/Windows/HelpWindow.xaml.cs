using Stand.General.Entities;
using Stand.General.Insrastructure.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Stand.UI.Windows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<Command> _commands;
        private string _currentCommandDescription;

        public HelpWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.LoadCommands();
        }

        public string CurrentCommandDescription
        {
            get { return _currentCommandDescription; }
            set
            {
                if (_currentCommandDescription != value)
                {
                    _currentCommandDescription = value;
                    OnPropertyChanged("CurrentCommandDescription");
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void LoadCommands()
        {
            var commandProvider = new CommandProvider();
            _commands = commandProvider.GetCommands();
            var commandNames = commandProvider.GetAllCommandNames();
            this.FillCommands(commandNames);
        }

        private void FillCommands(IEnumerable<string> commandNames)
        {
            foreach (var commandName in commandNames)
            {
                var commandNameControl = new TextBlock
                {
                    Text = commandName,
                    TextWrapping = TextWrapping.Wrap,
                    FontWeight = FontWeights.Normal,
                    Cursor = Cursors.Hand
                };

                commandNameControl.PreviewMouseLeftButtonUp += CommandNameControl_PreviewMouseLeftButtonUp;
                commandNameControl.MouseEnter += CommandNameControl_MouseEnter;
                commandNameControl.MouseLeave += CommandNameControl_MouseLeave;

                commandsStackPanel.Children.Add(commandNameControl);
            }
        }

        private void CommandNameControl_MouseLeave(object sender, MouseEventArgs e)
        {
            var textBlock = (sender as TextBlock);
            textBlock.Foreground = Brushes.Black;
            textBlock.TextDecorations = null;
        }

        private void CommandNameControl_MouseEnter(object sender, MouseEventArgs e)
        {
            var textBlock = (sender as TextBlock);
            textBlock.Foreground = Brushes.Blue;
            textBlock.TextDecorations = TextDecorations.Underline;
        }

        private void CommandNameControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var description = "Описание не найдено";

            var fullCommand = (sender as TextBlock).Text;
            var commandNames = fullCommand.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Command currentCommand = null;
            foreach (var commandName in commandNames)
            {
                currentCommand = currentCommand == null ?
                                    _commands.FirstOrDefault(x => x.Name == commandName) :
                                    currentCommand.Subcommands?.FirstOrDefault(x => x.Name == commandName);
            }

            if (currentCommand != null)
            {
                description = currentCommand.Description;
            }

            this.CurrentCommandDescription = description;
        }
    }
}
