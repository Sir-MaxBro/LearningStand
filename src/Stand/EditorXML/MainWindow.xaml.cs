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

namespace EditorXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XDocument _xdocument;
        private string _path;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _xdocument = new XDocument();
            _xdocument.Add(CreateEmpyElement("commands"));

        }
        private void ReadXML(string path)
        {

            try
            {
                _xdocument = XDocument.Load(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }
        }
        private void ReadXML()
        {
            ReadXML(System.AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\labs_tasks\\lab1.xml");
        }

        private void BuildNode(XDocument doc)
        {
            //userControl.header.Text = doc.Root.Name.LocalName;
            foreach (XElement child in doc.Root.Nodes())
            {
                NodeViewer node = new NodeViewer(child);
                Container.Children.Add(node);
                BuildNodes(node, child);
            }
        }
        private void BuildNodes(NodeViewer treeNode, XElement element)
        {

            foreach (XElement child in element.Nodes())
            {
                NodeViewer node = new NodeViewer(child);
                treeNode.stackPanel.Children.Add(node);
                BuildNodes(node, child);
            }
        }
        private XElement CreateEmpyElement()
        {
            var entry = new XElement("command");
            entry.Add(new XAttribute("name", "undef"));
            entry.Add(new XAttribute("description", "undefDef"));
            return entry;
        }
        private XElement CreateEmpyElement(string name )
        {
            XElement e = CreateEmpyElement();
            e.Name = name;
            return e;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XElement t = CreateEmpyElement();
            _xdocument.Root.Add(t);
            Container.Children.Add(new NodeViewer(t));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileStream f;
            if (_path == null)
            {
                System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();
                s.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
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
            else { 
            f= new FileStream(_path, FileMode.Create); 
            }
            
            _xdocument.Save(f);
            f.Close();
        }

        private void MenuItem_new_Click(object sender, RoutedEventArgs e)
        {
            _path = null;
            Container.Children.Clear();
            _xdocument = new XDocument();
            _xdocument.Add(CreateEmpyElement("commands"));
        }

        private void MenuItem_open_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog();
            f.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Container.Children.Clear();
                _path = f.FileName;
                ReadXML(_path);
                BuildNode(_xdocument);
            }
        }
    }
}
