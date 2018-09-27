using Stand.Domain.Abstract;
using Stand.Domain.Exceptions;
using Stand.UI.Exceptions;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Stand.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для TaskPanel.xaml
    /// </summary>
    public partial class TaskPanel : UserControl, INotifyPropertyChanged
    {
        private const string LAB_EXTENSION = ".xml";

        private readonly string PATH_TO_LABS = System.AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\labs_tasks";

        public event PropertyChangedEventHandler PropertyChanged;

        private IList<TaskControl> _tasks = new List<TaskControl>();
        private TaskControl _currentTask;
        private ICompiler _compiler;
        private int _currentIndex = -1;

        public TaskPanel()
        {
            InitializeComponent();
            this.DataContext = this;

            DirectoryInfo directory = new DirectoryInfo(PATH_TO_LABS);
            var listFiles = directory.GetFiles("*" + LAB_EXTENSION)
                .Select(file => file.Name.Substring(0, file.Name.Length - LAB_EXTENSION.Length));
            labsComboBox.ItemsSource = listFiles;
        }

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public IList<TaskControl> Tasks
        {
            get { return _tasks; }
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    this.OnPropertyChanged("Tasks");
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
            if (_currentTask.RightTaskCommand.ToLower() == command.Trim().ToLower())
            {
                checkCommand = true;
                this.CurrentTaskComplete();
                this.NextCurrent();
            }
            else if (_compiler != null)
            {
                var validResult = _compiler.IsValid(command);
                if (!validResult.IsValid)
                {
                    StringBuilder errorMessage = new StringBuilder("Команда не соответствует заданию.\n");
                    errorMessage.AppendLine("Может вы имели ввиду: '" + validResult.MostSimilarCommand + "'");
                    throw new CommandNotMatchAssignment(errorMessage.ToString());
                }
            }

            return checkCommand;
        }

        private void Labs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                string fileName = (sender as ComboBox).SelectedItem.ToString();
                string fullPath = PATH_TO_LABS + "\\" + fileName + LAB_EXTENSION;
                this.LoadTasks(fullPath);
            }
        }

        private void LoadTasks(string path)
        {
            XDocument xdocument = new XDocument();
            try
            {
                xdocument = XDocument.Load(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }

            var commands = xdocument.Elements("Commands").Elements("Command").ToList();

            _tasks.Clear();

            foreach (var command in commands)
            {
                var taskControl = new TaskControl
                {
                    Description = command.Attribute("description").Value,
                    RightTaskCommand = command.Attribute("name").Value,
                    IsDone = false
                };
                _tasks.Add(taskControl);
            }

            taskStackPanel.Items.Refresh();
            this.NextCurrent();
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
    }
}
