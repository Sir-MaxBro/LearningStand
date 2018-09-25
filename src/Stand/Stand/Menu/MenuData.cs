using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Stand.UI.Menu
{
    public static class MenuData
    {
        private static string[] _extensions = { ".pdf", ".txt", ".djvu", ".doc", ".docx", ".fb2" };
        private static readonly string _pathToLabs = Directory.GetCurrentDirectory() + "\\Labs";

        private static FileSystemInfo[] _fileSystemInfo;

        public static string[] GetMenuItems()
        {
            return _fileSystemInfo
                .Where(x => _extensions.Contains(x.Extension))
                .Select(x => x.Name)
                .ToArray();
        }

        public static string PathToLabs
        {
            get { return _pathToLabs; }
        }

        static MenuData()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_pathToLabs);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            _fileSystemInfo = directoryInfo.GetFileSystemInfos();
        }

        public static void OpenFile(string name)
        {
            try
            {
                System.Diagnostics.Process.Start(PathToLabs + "\\" + name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть файл", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
