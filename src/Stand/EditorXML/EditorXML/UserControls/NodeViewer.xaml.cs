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

namespace EditorXML.UserControls
{
    /// <summary>
    /// Interaction logic for NodeViewer.xaml
    /// </summary>
    public partial class NodeViewer : UserControl, INotifyPropertyChanged
    {
     
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly XDocument _xdocument;
        private List<NodeViewer> _commandCollection;
        private XElement _element;

        public XElement Element
        {
            get { return _element; }
           /* set { _element = value; }*/
        }
        public NodeViewer(XElement element):base()
        {
            InitializeComponent();
            this.DataContext = this;
            _element = element;
            var p =  element.Attributes();
            var t = p.FirstOrDefault(s => s.Name == "name");
            header.Text =t.Value;
            descr.Text = element.Attributes().FirstOrDefault(s => s.Name == "description").Value;

        }
        public NodeViewer()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            //Expander expander = new Expander();
            XElement childElement = new XElement(_element);
               var t = new NodeViewer(childElement);
                  stackPanel.Children.Add(t);
                  _element.Add(childElement);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Content = null;
            _element.Remove();
        }

        private void header_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_element != null) 
            _element.Attributes().FirstOrDefault(s => s.Name == "name").Value = (sender as TextBox).Text;
        }

        private void descr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_element != null) 
            _element.Attributes().FirstOrDefault(s => s.Name == "description").Value = (sender as TextBox).Text;
        }

    }
}
