using EditorXML.Domain.Entity;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace EditorXML.Windows
{
    /// <summary>
    /// Interaction logic for EditCommand.xaml
    /// </summary>
    public partial class EditCommandWindow : Window
    {
        Command command;
        public bool IsRemove { get; set; }
        public Command Command
        {
            get { return command; }
            set { command = value; }
        }

        public EditCommandWindow()
        {
            InitializeComponent();
            command = new Command();
            this.DataContext = this;
            IsRemove = false;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            IsRemove = true;
            this.Close();
        }


    }
}
