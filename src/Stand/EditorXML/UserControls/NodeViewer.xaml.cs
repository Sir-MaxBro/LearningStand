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
using EditorXML.Domain.Service;
using EditorXML.Windows;
using EditorXML.Domain.Entity;

namespace EditorXML.UserControls
{
    /// <summary>
    /// Interaction logic for NodeViewer.xaml
    /// </summary>
    public partial class NodeViewer : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ComboBoxSelectionChanged;

        private List<Command> _commands;

        public List<Command> Commands
        {
            get { return _commands; }
            set { _commands = value; }

        }
        public NodeViewer()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void ComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ComboBox combobox = sender as ComboBox;
            if (combobox.SelectedItem is Command)
            {
                Command selectedCommand = combobox.SelectedItem as Command;
                Command uneditedCommand = selectedCommand.Clone() as Command;
                EditCommandWindow editCommandWindow = new EditCommandWindow();
                editCommandWindow.Command = selectedCommand;
                if (editCommandWindow.ShowDialog() == true)
                {
                    if (editCommandWindow.IsRemove)
                    {
                        var index = _commands.IndexOf(selectedCommand);
                        _commands.Remove(selectedCommand);
                    }
                }
                else
                {
                    var index = _commands.IndexOf(selectedCommand);
                    if (index != -1)
                    {

                        _commands[index] = uneditedCommand;
                    }
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
            EditCommandWindow addCommandWindow = new EditCommandWindow();

            if (addCommandWindow.ShowDialog() == true)
            {
                _commands.Add(addCommandWindow.Command);
                OnPropertyChanged("Commands");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));

            }
        }


    }
}
