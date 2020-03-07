using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using Framework.Desktop;
using Framework.Utils;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace Framework.Appium
{
    public class AppClient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AppiumLocalService service;
        private readonly Settings settings;

        public AppClient(AppiumLocalService service, Settings settings)
        {
            this.settings = settings;
            this.service = service;
        }

        public WindowsDriver<WindowsElement> Driver { get; set; }

        public void Start()
        {
            // Verify Appium server is running
            Assert.IsTrue(service.IsRunning, "Appium Server is not running!");

            // Verify Application exists
            Assert.IsTrue(File.Exists(settings.AppPath), "Application do not exists at: " + settings.AppPath);

            // Create a new Appium session to launch application under test
            AppiumOptions options = new AppiumOptions();
            options.AddAdditionalCapability("app", settings.AppPath);
            options.AddAdditionalCapability("deviceName", "WindowsPC");
            options.AddAdditionalCapability("ms:experimental-webdriver", true);

            // Add newCommandTimeout if debugger is attached to allow debugging for more than 60 seconds.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                options.AddAdditionalCapability("newCommandTimeout", 3600);
            }

            // Start Appium clinet (and application under test)
            Driver = new WindowsDriver<WindowsElement>(service.ServiceUrl, options);
            Log.Info("Start Appium session to app under test.");

            // Set implicit timeout to auto wait for element when try to find it
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.Timeout);

            // Wait for splash screen to disappear
            if (settings.AppTitle != null)
            {
                var titleFound = Wait.Until(() => GetTitle().Contains(settings.AppTitle), timeout: settings.Timeout);
                if (!titleFound)
                {
                    FileSystem.CreateFolder(settings.TestResultsFolder);
                    var failedScreenPath = Path.Combine(settings.TestResultsFolder, "desktop.png");
                    ImageUtils.SaveScreenshot(failedScreenPath);
                    Assert.Fail("Failed to find window with name: " + settings.AppTitle);
                }
            }

            // Set app possition and size
            if (settings.Size != null)
            {
                Driver.Manage().Window.Position = new Point(0, 0);
                Driver.Manage().Window.Size = (Size)settings.Size;
            }
            else
            {
                Driver.Manage().Window.Maximize();
            }
        }

        public void Stop()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Log.Info("Stop Appium session to app under test.");
            }
        }

        private string GetTitle()
        {
            try
            {
                if (Driver.WindowHandles.Count > 0)
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[0]);
                    return Driver.Title;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
