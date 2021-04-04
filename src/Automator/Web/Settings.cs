using System.Drawing;
using System.Runtime.InteropServices;
using Automator.Shared.Utils;
using Automator.Shared.VisualTesting;
using Automator.Web.Enums;
using Microsoft.Extensions.Configuration;

namespace Automator.Web
{
    /// <summary>
    /// Test settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            IConfiguration config = Config.GetConfiguration();

            BaseUrl = Config.GetEnvironmentVariable("BASE_URL", config.GetValue<string>("BaseUrl"));
            Timeout = Config.GetEnvironmentVariable("TIMEOUT", config.GetValue<int>("Timeout"));

            var browserSection = config.GetSection("Browser");
            BrowserType = Config.GetEnvironmentVariable("BROWSER_TYPE", browserSection.GetValue<BrowserType>("Type"));
            Headless = Config.GetEnvironmentVariable("HEADLESS", browserSection.GetValue<bool>("Headless"));

            var browserSizeSettings = browserSection.GetSection("Size");
            BrowserSize = browserSizeSettings == null
                ? Size.Empty
                : new Size(browserSizeSettings.GetValue<int>("Width"), browserSizeSettings.GetValue<int>("Height"));

            OSType = Config.GetOSType();
            VisualTesting = new VisualTestingSettings(config.GetSection("VisualTesting"));
        }

        /// <summary>
        /// Gets base URL for web pages.
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// Gets standard timeout used to find elements (in seconds).
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Gets <see cref="BrowserType"/> of browser used in tests.
        /// </summary>
        public BrowserType BrowserType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether browser should run in head.
        /// </summary>
        public bool Headless { get; private set; }

        /// <summary>
        /// Gets a value indicating default browser size.
        /// </summary>
        public Size BrowserSize { get; private set; }

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
