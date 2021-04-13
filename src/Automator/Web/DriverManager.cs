using System;
using System.Drawing;
using Automator.Shared.Utils;
using Automator.Web.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using WebDriverManager.DriverConfigs.Impl;

namespace Automator.Web
{
    /// <summary>
    /// Setup browser driver.
    /// </summary>
    internal class DriverManager
    {
        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverManager"/> class.
        /// </summary>
        /// <param name="settings">Instance of <see cref="Settings"/>.</param>
        internal DriverManager(Settings settings)
        {
            this.settings = settings;

            Driver = ((object)settings.BrowserType) switch
            {
                BrowserType.Chrome => ChromeDriver(),
                BrowserType.Firefox => FirefoxDriver(),
                BrowserType.Safari => SafariDriver(),
                _ => throw new InvalidOperationException($"{settings.BrowserType} is not supported browser type."),
            };
        }

        internal IWebDriver Driver { get; private set; }

        /// <summary>
        /// Creates <see cref="OpenQA.Selenium.Chrome.ChromeDriver"/> instance.
        /// </summary>
        /// <returns>A new instance of <see cref="OpenQA.Selenium.Chrome.ChromeDriver"/>.</returns>
        private IWebDriver ChromeDriver()
        {
            // Download and configure driver binary.
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());

            // Construct Chrome options.
            var options = new ChromeOptions();
            options.AddArgument("--disable-gpu");
            options.AddArgument("--force-device-scale-factor=1");
            options.AddArgument("--ignore-certificate-errors");
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            options.AddUserProfilePreference("download.default_directory", FileSystem.ProjectRoot);

            if (settings.Headless)
            {
                options.AddArguments("headless");
            }

            if (settings.BrowserSize != Size.Empty)
            {
                options.AddArgument($"--window-size={settings.BrowserSize.Width},{settings.BrowserSize.Height}");
            }
            else
            {
                options.AddArgument("--start-maximized");
            }

            // Init driver (start browser).
            return new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
        }

        /// <summary>
        /// Creates <see cref="OpenQA.Selenium.Firefox.FirefoxDriver"/> instance.
        /// </summary>
        /// <returns>A new instance of <see cref="OpenQA.Selenium.Firefox.FirefoxDriver"/>.</returns>
        private IWebDriver FirefoxDriver()
        {
            // Download and configure driver binary.
            new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());

            // Construct firefox profile
            var profile = new FirefoxProfile();
            profile.SetPreference("devtools.console.stdout.content", true);

            // Construct Firefox options.
            FirefoxOptions options = new FirefoxOptions
            {
                AcceptInsecureCertificates = true,
                Profile = profile
            };

            options.SetPreference("browser.download.folderList", 2);
            options.SetPreference("browser.download.dir", FileSystem.ProjectRoot);

            if (settings.Headless)
            {
                options.AddArguments("-headless");
            }

            if (settings.BrowserSize != Size.Empty)
            {
                options.AddArguments("-width=" + settings.BrowserSize.Width);
                options.AddArguments("-height=" + settings.BrowserSize.Height);
            }

            // Create FirefoxDriverService and customize Host to prevent performance issues.
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";

            // Init driver (start browser).
            return new FirefoxDriver(service, options, TimeSpan.FromMinutes(3));
        }

        /// <summary>
        /// Creates <see cref="OpenQA.Selenium.Safari.SafariDriver"/> instance.
        /// </summary>
        /// <returns>A new instance of <see cref="OpenQA.Selenium.Safari.SafariDriver"/>.</returns>
        private IWebDriver SafariDriver()
        {
            // Note:
            // Safari driver is part of macOS operating system.
            // No need to setup with WebDriverManager.

            // Init driver (start browser)
            var options = new SafariOptions();
            var driver = new SafariDriver(SafariDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));

            // Set size at runtime (no way to do it with capabilities)
            driver.Manage().Window.Size = settings.BrowserSize;

            // Return the driver
            return driver;
        }
    }
}
