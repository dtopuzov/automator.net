using Microsoft.Extensions.Configuration;

namespace Automator
{
    public class VisualSettings
    {
        public VisualSettings(IConfiguration config)
        {
            var settings = config.GetSection("Visual");
            Tolerance = Config.GetSetting("TOLERANCE", settings.GetValue("Tolerance", 0.01d));
            VerificationType = Config.GetSetting("VERIFICATION_TYPE", settings.GetValue("VerificationType", VerificationType.CompareImages));
            ImagePath = GetImagePath(settings);
        }

        public double Tolerance { get; set; }

        public string ImagePath { get; set; }

        public VerificationType VerificationType { get; set; }

        private string GetImagePath(IConfigurationSection settings)
        {
            var pathValue = Config.GetSetting("IMAGE_PATH", settings.GetValue<string>("ImagePath", "Images"));
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
    }
}
