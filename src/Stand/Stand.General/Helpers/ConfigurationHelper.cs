using System;
using System.Configuration;

namespace Stand.General.Helpers
{
    public static class ConfigurationHelper
    {
        public static Configuration GetExecutingAssemblyConfig<T>(T obj)
        {
            var dllPath = new Uri(obj.GetType().Assembly.GetName().CodeBase).LocalPath;
            return ConfigurationManager.OpenExeConfiguration(dllPath);
        }
    }
}
