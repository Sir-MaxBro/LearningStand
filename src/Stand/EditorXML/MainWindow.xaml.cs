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

namespace EditorXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const String EXTENSION_FILTER = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        private XDocument _document;
        private string _path;
        private IXmlService _xmlService = new XmlService();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            _document = _xmlService.GetDefaultDocument();

        }

        private void AddElement_Button_Click(object sender, RoutedEventArgs e)
        {
            XElement newXmlElement = _xmlService.GetDefaultElement();
            _document.Root.Add(newXmlElement);
            Container.Children.Add(new NodeViewer(newXmlElement));

        }

        private void New_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _path = null;
            Container.Children.Clear();
            _document = _xmlService.GetDefaultDocument();
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
                SaveInNewFilePath(saveFileDialog.FileName);
            }
        }

        private void SaveInNewFilePath(String path)
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
            FileStream fileToSave = new FileStream(path, FileMode.Create);

            _document.Save(fileToSave);
            fileToSave.Close();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = EXTENSION_FILTER;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Container.Children.Clear();
                _path = openFileDialog.FileName;
                _document = _xmlService.ReadFile(_path);
                ViewXml(_document);
            }
        }

        //todo it's 2 similar methods
        private void ViewXml(XDocument document)
        {
            foreach (XElement child in document.Root.Nodes())
            {
                NodeViewer node = new NodeViewer(child);
                //todo bind it 
                Container.Children.Add(node);
                ViewXmlNodes(node, child);
            }
        }
        private void ViewXmlNodes(NodeViewer treeNode, XElement element)
        {
            foreach (XElement child in element.Nodes())
            {
                NodeViewer node = new NodeViewer(child);
                //   treeNode.stackPanel.Children.Add(node);
                ViewXmlNodes(node, child);
            }
        }
    }
}
