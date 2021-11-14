using Microsoft.Extensions.Configuration;

namespace Automator
{
    public class DeviceSettings
    {
        public DeviceSettings(IConfiguration config)
        {
            var settings = config.GetSection("Device");
            PlatformType = Config.GetSetting("PLATFORM_TYPE", settings.GetValue<string>("PlatformType"));
            PlatformVersion = Config.GetSetting("PLATFORM_VERSION", settings.GetValue<string>("PlatformVersion"));
            DeviceType = Config.GetSetting("DEVICE_TYPE", settings.GetValue<DeviceType>("DeviceType"));
            DeviceName = Config.GetSetting("DEVICE_NAME", settings.GetValue<string>("DeviceName"));
            DeviceId = Config.GetSetting("DEVICE_ID", settings.GetValue<string>("DeviceId"));
        }

        public string PlatformType { get; set; }

        public string PlatformVersion { get; set; }

        public DeviceType DeviceType { get; set; }

        public string DeviceName { get; set; }

        public string DeviceId { get; set; }
    }
}
