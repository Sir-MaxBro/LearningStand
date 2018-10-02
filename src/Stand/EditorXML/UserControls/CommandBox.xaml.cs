using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using EditorXML.Domain.Abstract;
using EditorXML.Domain.DAO;
using EditorXML.Windows;
using EditorXML.Domain.Entity;
using System.Collections.ObjectModel;

namespace EditorXML.UserControls
{
    /// <summary>
    /// Interaction logic for NodeViewer.xaml
    /// </summary>
    public partial class CommandBox : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public event SelectionChangedEventHandler ComboBoxSelectionChanged;

        private IList<Command> _commands;

        public ObservableCollection<Command> Commands
        {
            get { return new ObservableCollection<Command>(_commands); }
            set { _commands = value; }
        }

        public CommandBox(IList<Command> commands)
            : base()
        {
            InitializeComponent();
            this.DataContext = this;
            this._commands = commands;

        }

        private void ComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ComboBox combobox = sender as ComboBox;
            if (combobox.SelectedItem is Command)
            {
                Command selectedCommand = combobox.SelectedItem as Command;
                int index = _commands.IndexOf(selectedCommand);
                Command commandForEdit = selectedCommand.Clone() as Command;
                EditCommandWindow editCommandWindow = new EditCommandWindow();

                editCommandWindow.Command = commandForEdit;

                if (editCommandWindow.ShowDialog() == true)
                {
                    if (editCommandWindow.IsRemoved)
                    {
                        _commands.Remove(selectedCommand);
                    }
                    else
                    {
                        if (!IsCommandExists(commandForEdit))
                        {
                            _commands[index] = commandForEdit;
                            combobox.SelectedItem = commandForEdit;
                        }
                    }
                    OnPropertyChanged("Commands");
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ComboBoxSelectionChanged != null)
            {
                this.ComboBoxSelectionChanged(this, e);
            }
        }

        private void AddCommandButton_Click(object sender, RoutedEventArgs e)
        {
            AddCommandWindow addCommandWindow = new AddCommandWindow();

            if (addCommandWindow.ShowDialog() == true)
            {
                if (!IsCommandExists(addCommandWindow.Command))
                {
                    _commands.Add(addCommandWindow.Command);

                    CommandCombobox.SelectedItem = addCommandWindow.Command;
                    OnPropertyChanged("Commands");
                }
            }
        }
        private bool IsCommandExists(Command command)
        {
            bool result = _commands.FirstOrDefault(comm => comm.Name == command.Name) != null;
            if (result)
            {
                MessageBox.Show("command already exists.Changes doesn't save");
            }
            return result;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));

            }
        }


    }
}
