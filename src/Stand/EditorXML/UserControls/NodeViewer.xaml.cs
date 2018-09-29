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

namespace EditorXML.UserControls
{
    /// <summary>
    /// Interaction logic for NodeViewer.xaml
    /// </summary>
    public partial class NodeViewer : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private XElement _element;

        public XElement Element
        {
            get { return _element; }
        }
        public NodeViewer(XElement element)
            : base()
        {
            InitializeComponent();
            this.DataContext = this;
            _element = element;

            XAttribute nameAttribute = element.Attribute(XName.Get("name"));
            header.Text = nameAttribute.Value;

        }
        public NodeViewer()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void AddNodeButton_Click(object sender, RoutedEventArgs e)
        {
            XElement childElement = new XElement(_element);
            var t = new NodeViewer(childElement);
            //  stackPanel.Children.Add(t);
            _element.Add(childElement);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
            _element.Remove();
        }

        private void header_TextChanged(object sender, TextChangedEventArgs e)
        {
            XAttribute nameAttribute = _element.Attribute(XName.Get("name"));
            nameAttribute.Value = (sender as TextBox).Text;
        }


    }
}
