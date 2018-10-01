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
using EditorXML.Domain.Service;
using System.Windows.Forms;
using System.Xml.Serialization;
using EditorXML.Domain.Entity;

namespace EditorXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const String EXTENSION_FILTER = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        private string _path;
        private List<Command> _commands;
        private ICommandService _xmlService = new CommandService();


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            _commands = new List<Command>();

        }

        private void AddElement_Button_Click(object sender, EventArgs e)
        {
            var tt = e as SelectionChangedEventArgs;
          Command selectedCommand=  (tt.Source as System.Windows.Controls.ComboBox).SelectedItem as Command;
            var enumerator = Container.Children.IndexOf(sender as NodeViewer);
            if (enumerator != Container.Children.Count - 1)
            {
                Container.Children.RemoveRange(enumerator+1, Container.Children.Count - 1);
            }
            NodeViewer node = new NodeViewer();
            node.Commands = selectedCommand.SubCommands;
            Container.Children.Add(node);
            node.ComboBoxSelectionChanged += new EventHandler(AddElement_Button_Click);

        }

        private void New_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _path = null;
            Container.Children.Clear();
            _commands = new List<Command>();
            ViewXmlNode();
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

        private void SaveInNewFile(String path)
        {
            SaveInFile(path);
            _path = path;
        }

        private void SaveInFile()
        {
            SaveInFile(_path);
        }

        private void SaveInFile(String path)
        {
            _xmlService.Save(_commands, path);
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = EXTENSION_FILTER;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Container.Children.Clear();
                _path = openFileDialog.FileName;
                _commands = _xmlService.ReadCommandsFromFile(_path);
                ViewXmlNode();
            }
        }

        private void ViewXmlNode()
        {
            NodeViewer node = new NodeViewer();
            node.Commands = _commands;
            Container.Children.Add(node);

            node.ComboBoxSelectionChanged += new EventHandler(AddElement_Button_Click);
        }
    }
}
