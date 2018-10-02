using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;
using EditorXML.UserControls;
using EditorXML.Domain;
using EditorXML.Domain.Abstract;
using EditorXML.Domain.DAO;
using System.Windows.Forms;
using System.Xml.Serialization;
using EditorXML.Domain.Entity;
using System.Collections.ObjectModel;

namespace EditorXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const String EXTENSION_FILTER = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        private string _path;
        private ObservableCollection<Command> _commands;
        private ICommandDAO _xmlService = new XmlCommandDAO();


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            _commands = new ObservableCollection<Command>();

        }

        private void New_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _path = null;
            Container.Children.Clear();
            _commands = new ObservableCollection<Command>();
            ViewCommands(_commands);
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = EXTENSION_FILTER;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Container.Children.Clear();
                _path = openFileDialog.FileName;
                _commands = new ObservableCollection<Command>(_xmlService.LoadCommands(_path));
                ViewCommands(_commands);
            }
        }

        private void Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_path == null)
            {
                SaveAs_MenuItem_Click(sender, e);
            }
            else
            {
                SaveInFile(_path);
            }
        }

        private void SaveAs_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = EXTENSION_FILTER;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveInNewFile(saveFileDialog.FileName);
            }

        }

        private void SaveInFile(String path)
        {
            _xmlService.Save(_commands.ToList(), path);
        }

        private void SaveInNewFile(String path)
        {
            SaveInFile(path);
            _path = path;
        }

        private void ViewCommands(IList<Command> commands)
        {
            CommandBox commandBox = new CommandBox(commands);
            Container.Children.Add(commandBox);
            commandBox.ComboBoxSelectionChanged += new SelectionChangedEventHandler(ChangeSelectedCommand);
        }

        private void ChangeSelectedCommand(object sender, EventArgs e)
        {
            var changedCommandEventArgs = e as SelectionChangedEventArgs;
            Command selectedCommand = (changedCommandEventArgs.Source as System.Windows.Controls.ComboBox).SelectedItem as Command;

            var selectedPosition = Container.Children.IndexOf(sender as CommandBox);
            if (selectedPosition != Container.Children.Count - 1)
            {
                int count = Container.Children.Count - selectedPosition;
                Container.Children.RemoveRange(selectedPosition + 1, count);
            }
            if (selectedCommand != null)
            {
                ViewCommands(selectedCommand.SubCommands);
            }
        }
    }
}
