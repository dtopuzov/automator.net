namespace Automator
{
    public class Settings
    {
        public Settings(string? appsettings = null)
        {
            var config = appsettings == null
               ? Config.GetDefaultConfiguration()
               : Config.GetConfigurationFromFile(appsettings);
            App = new AppSettings(config);
            Device = new DeviceSettings(config);
            General = new GeneralSettings(config);
            Visual = new VisualSettings(config);
        }

        public AppSettings App { get; set; }

        public DeviceSettings Device { get; set; }

        public GeneralSettings General { get; set; }

        public VisualSettings Visual { get; set; }
    }
}
