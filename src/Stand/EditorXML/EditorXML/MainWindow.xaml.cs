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

        #region treeView
        /*
         *     private void BuildTree(TreeView treeView, XDocument doc)
        {
           
            TreeViewItem treeNode = new TreeViewItem
            {
                //Should be Root
                Header = doc.Root.Name.LocalName,
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            BuildNodes(treeNode, doc.Root);
        }
        private void BuildNodes(TreeViewItem treeNode, XElement element)
        {
            foreach (XNode child in element.Nodes())
            {
                switch (child.NodeType)
                {
                    case XmlNodeType.Element:
                        XElement childElement = child as XElement;
                        TreeViewItem childTreeNode = new TreeViewItem
                        {
                            //Get First attribute where it is equal to value
                            Header = childElement.Attributes().First(s => s.Name == "name").Value,
                            //Automatically expand elements
                            IsExpanded = true,
                            Tag = childElement.Attributes().First(s => s.Name == "description").Value
                        };
                        treeNode.Items.Add(childTreeNode);
                        BuildNodes(childTreeNode, childElement);
                        break;
                    case XmlNodeType.Text:
                        XText childText = child as XText;
                        treeNode.Items.Add(new TreeViewItem { Header = childText.Value, });
                        break;
                }
            }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private TreeViewItem _selectedNode;
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _selectedNode = (sender as TreeView).SelectedItem as TreeViewItem;
            if (_selectedNode == null || _selectedNode.Tag == null)
            {
                return;
            }
            Description = _selectedNode.Tag.ToString();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var command = e.OriginalSource as MenuItem;
            switch (command.Header.ToString().ToLower())
            {
                case "add":
                    if (_selectedNode == null)
                    {
                        treeView.Items.Add(new TreeViewItem() { Header = "new undef", Tag = "undef def" });
                        break;
                    }
                    _selectedNode.Items.Add(new TreeViewItem() { Header = "new undef", Tag = "undef def" });
                    break;
                case "delete":
                    DependencyObject dop = VisualTreeHelper.GetParent(_selectedNode);
                    break;
            }
        }*/
        #endregion
    }
}
