using System;
using System.IO;
using System.Linq;

namespace Stand.UI.Menu
{
    public static class MenuData
    {
        private static string _pathToLabs = Directory.GetCurrentDirectory() + "\\Labs";
        private static FileSystemInfo[] _fileSystemInfo;
        private static string[] _extensions = { ".pdf", ".txt", ".djvu", ".doc", ".docx", ".fb2" };

        public static string[] Items
        {
            get
            {
                return _fileSystemInfo
                    .Where(x => _extensions.Contains(x.Extension))
                    .Select(x => x.Name)
                    .ToArray();
            }
        }

        public static string PathToLabs
        {
            get { return _pathToLabs; }
        }

        static MenuData()
        {
            DirectoryInfo dir = new DirectoryInfo(_pathToLabs);
            if (!dir.Exists)
            {
                dir.Create();
            }
            _fileSystemInfo = dir.GetFileSystemInfos();
        }

        public static void OpenFile(string name)
        {
            try
            {
                System.Diagnostics.Process.Start(PathToLabs + "\\" + name);
            }
            catch (Exception ex)
            { }
        }
    }
}
