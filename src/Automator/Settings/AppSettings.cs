using Microsoft.Extensions.Configuration;

namespace Automator
{
    public class AppSettings
    {
        public AppSettings(IConfiguration config)
        {
            var settings = config.GetSection("App");
            Id = Config.GetSetting("APP_ID", settings.GetValue<string>("Id"));
            Name = Config.GetSetting("APP_NAME", settings.GetValue<string>("Name"));
            Path = GetAppPath(settings);
            Args = Config.GetSetting("APP_ARGS", settings.GetValue("Args", string.Empty));
            Activity = Config.GetSetting("APP_ARGS", settings.GetValue("Activity", string.Empty));
            Width = Config.GetSetting("APP_WIDTH", settings.GetSection("Size").GetValue<int>("Width"));
            Height = Config.GetSetting("APP_HEIGHT", settings.GetSection("Size").GetValue<int>("Height"));
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Args { get; set; }

        public string Activity { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        private string GetAppPath(IConfigurationSection settings)
        {
            var pathValue = Config.GetSetting("APP_PATH", settings.GetValue<string>("Path"));
            if (pathValue != null)
            {
                var pathType = Config.GetSetting("PATH_TYPE", settings.GetValue<PathType>("PathType"));
                if (pathType != PathType.Relative)
                {
                    return pathValue;
                }
                else
                {
                    var binDebug = $"bin{System.IO.Path.DirectorySeparatorChar}Debug";
                    var projectRoot = AppDomain.CurrentDomain.BaseDirectory.Split(binDebug)[0];
                    return System.IO.Path.Combine(projectRoot, pathValue);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
