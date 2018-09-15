using Stand.Domain.Abstract;
using Stand.Domain.Exceptions;
using Stand.UI.Exceptions;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для TaskPanel.xaml
    /// </summary>
    public partial class TaskPanel : UserControl, INotifyPropertyChanged
    {
        private IList<TaskControl> _tasks = new List<TaskControl>();
        private int _currentIndex = -1;
        private TaskControl _currentTask;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly string LABS_PATH = System.AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\labs_tasks";
        private XDocument _xdocument;
        private ICompiler _compiler;
        public TaskPanel()
        {
            InitializeComponent();
            this.DataContext = this;

            DirectoryInfo directory = new DirectoryInfo(LABS_PATH);
            var listFiles = directory.GetFiles("*.xml")
                .Select(x => x.Name.Substring(0, x.Name.Length - 4));
            cmbLabs.ItemsSource = listFiles;
        }

        private void FillTasks(string path)
        {
            try
            {
                _xdocument = XDocument.Load(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }
            var commands = from xnode in _xdocument.Elements("Commands").Elements("Command")
                           select xnode;
            _tasks.Clear();

            foreach (var item in commands)
            {
                var taskControl = new TaskControl();
                taskControl.Description = item.Attribute("description").Value;
                taskControl.TaskCommand = item.Attribute("name").Value;
                taskControl.IsDone = false;
                _tasks.Add(taskControl);
            }
            tStack.Items.Refresh();
            NextCurrent();
        }

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public IList<TaskControl> Tasks
        {
            get { return _tasks; }
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    OnPropertyChanged("Tasks");
                }
            }
        }

        public TaskControl CurrentTask
        {
            get { return _currentTask; }
        }

        protected void CurrentTaskComplete()
        {
            _currentTask.IsDone = true;
            _currentTask.IsCurrent = false;
        }

        private void NextCurrent()
        {
            _currentIndex++;
            if (_tasks.Count > _currentIndex)
            {
                _currentTask = _tasks[_currentIndex];
                _currentTask.IsCurrent = true;
            }
        }

        public ICompiler Compiler
        {
            get { return _compiler; }
            set { _compiler = value; }
        }

        public bool CheckTask(string command)
        {
            bool checkCommand = false;

            if (_currentTask == null)
            {
                throw new EmptyTaskCommandsException("Задание не выбрано.");
            }

            command = command.Remove(0, command.IndexOf('#') + 1);
            if (_currentTask.TaskCommand.ToLower() == command.Trim().ToLower())
            {
                checkCommand = true;
                CurrentTaskComplete();
                NextCurrent();
            }
            else if (_compiler != null && _compiler.IsValid(command).IsValid)
            {
                throw new CommandNotMatchAssignment("Команда не соответствует заданию");
            }

            return checkCommand;
        }

        private void Labs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                string fileName = (sender as ComboBox).SelectedItem.ToString();
                string fullPath = LABS_PATH + "\\" + fileName + ".xml";
                FillTasks(fullPath);
            }
        }
    }
}
