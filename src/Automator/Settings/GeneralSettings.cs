using Microsoft.Extensions.Configuration;

namespace Automator
{
    public class GeneralSettings
    {
        public GeneralSettings(IConfiguration config)
        {
            var settings = config.GetSection("General");
            Timeout = Config.GetSetting("TIMEOUT", settings.GetValue("Timeout", 30));
            HostOS = Config.GetSetting("HOST_OS", settings.GetValue<string>("HostOS"));
        }

        public int Timeout { get; set; }

        public string HostOS { get; set; }
    }
}
