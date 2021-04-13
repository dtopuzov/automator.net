using System.Runtime.InteropServices;
using Automator.Shared.Utils;
using Automator.Shared.VisualTesting;
using Microsoft.Extensions.Configuration;

namespace Automator.Mobile
{
    public class MobileSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileSettings"/> class.
        /// </summary>
        public MobileSettings()
        {
            IConfiguration config = Config.GetConfiguration();
            Timeout = config.GetValue("Timeout", 30);

            var appiumSection = config.GetSection("Appium");
            Platform = appiumSection.GetValue<string>("Platform");
            PlatformVersion = appiumSection.GetValue<string>("PlatformVersion");
            DeviceName = appiumSection.GetValue<string>("DeviceName");
            App = appiumSection.GetValue<string>("App");
            DeviceId = appiumSection.GetValue<string>("DeviceId");
            Avd = appiumSection.GetValue<string>("Avd");
            AvdArgs = appiumSection.GetValue<string>("AvdArgs");
            ChromeDriverVersion = appiumSection.GetValue<string>("ChromeDriverVersion");

            OSType = Config.GetOSType();
            VisualTesting = new VisualTestingSettings(config.GetSection("VisualTesting"));
        }

        /// <summary>
        /// Gets standard timeout used to find elements (in seconds).
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Gets mobile platform type.
        /// </summary>
        public string Platform { get; private set; }

        /// <summary>
        /// Gets version of the mobile OS.
        /// </summary>
        public string PlatformVersion { get; private set; }

        /// <summary>
        /// Gets device name.
        /// </summary>
        public string DeviceName { get; private set; }

        /// <summary>
        /// Gets path to app under test.
        /// </summary>
        public string App { get; private set; }

        /// <summary>
        /// Gets unique device identifier.
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// Gets android emulator name.
        /// </summary>
        public string Avd { get; private set; }

        /// <summary>
        /// Gets android emulator startup options.
        /// </summary>
        public string AvdArgs { get; private set; }

        /// <summary>
        /// Gets version of chromedriver.
        /// </summary>
        public string ChromeDriverVersion { get; private set; }

        /// <summary>
        /// Gets host OS type.
        /// </summary>
        public OSPlatform OSType { get; private set; }

        /// <summary>
        /// Gets visual testing settings.
        /// </summary>
        public VisualTestingSettings VisualTesting { get; private set; }
    }
}
