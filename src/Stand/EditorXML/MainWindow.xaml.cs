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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XElement newXmlElement = _xmlService.GetDefaultElement();
            _document.Root.Add(newXmlElement);
            Container.Children.Add(new NodeViewer(newXmlElement));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileStream f;
            if (_path == null)
            {
                System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();
                s.Filter = EXTENSION_FILTER;
                if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    f = new FileStream(s.FileName, FileMode.Create);
                    _path = s.FileName;
                }
                else
                {
                    return;
                }
            }
            else
            {
                f = new FileStream(_path, FileMode.Create);
            }

            _document.Save(f);
            f.Close();
        }
        //todo rename it
        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            _path = null;
            Container.Children.Clear();
            _document = _xmlService.GetDefaultDocument();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog();
            f.Filter = EXTENSION_FILTER;

            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Container.Children.Clear();
                _path = f.FileName;
                _xmlService.ReadFile(_path);
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
