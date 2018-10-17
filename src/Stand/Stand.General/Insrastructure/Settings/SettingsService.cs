using Stand.General.Helpers;
using System.Configuration;

namespace Stand.General.Insrastructure.Settings
{
    public class SettingsService
    {
        private static SettingsService _instance;
        private Configuration _configuration;

        private SettingsService()
        {
            this.RefreshConfiguration();
        }

        public static SettingsService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SettingsService();
            }
            return _instance;
        }

        public Entities.Settings GetSettings()
        {
            Entities.Settings settings = null;
            var appSettings = _configuration.AppSettings.Settings;

            if (appSettings != null)
            {
                settings = new Entities.Settings
                {
                    PathToCommands = appSettings[SettingsConstants.PathToCommandsKey].Value,
                    PathToLabs = appSettings[SettingsConstants.PathToLabsKey].Value,
                    PathToTasks = appSettings[SettingsConstants.PathToTasksKey].Value,
                    ConnectionTimeOut = int.Parse(appSettings[SettingsConstants.ConnectionTimeOutKey].Value),
                    ErrorTimeOut = int.Parse(appSettings[SettingsConstants.ErrorTimeOutKey].Value)
                };
            }

            return settings;
        }

        public void SetSettings(Entities.Settings settings)
        {
            if (settings != null)
            {
                var appSettings = _configuration.AppSettings.Settings;

                appSettings[SettingsConstants.PathToCommandsKey].Value = settings.PathToCommands;
                appSettings[SettingsConstants.PathToLabsKey].Value = settings.PathToLabs;
                appSettings[SettingsConstants.PathToTasksKey].Value = settings.PathToTasks;
                appSettings[SettingsConstants.ConnectionTimeOutKey].Value = settings.ConnectionTimeOut.ToString();
                appSettings[SettingsConstants.ErrorTimeOutKey].Value = settings.ErrorTimeOut.ToString();

                _configuration.Save(ConfigurationSaveMode.Modified);

                this.RefreshConfiguration();
            }
        }

        private void RefreshConfiguration()
        {
            _configuration = ConfigurationHelper.GetExecutingAssemblyConfig(this);
        }
    }
}
