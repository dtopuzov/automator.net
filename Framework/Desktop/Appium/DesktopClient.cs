using System;
using System.Reflection;
using log4net;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace Framework.Desktop
{
    public class DesktopClient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly AppiumLocalService service;
        private readonly Settings settings;

        public DesktopClient(AppiumLocalService service, Settings settings)
        {
            this.service = service;
            this.settings = settings;
        }

        public WindowsDriver<WindowsElement> Driver { get; protected set; }

        public void Start()
        {
            // Create a new Appium session to control Windows desktop
            AppiumOptions options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");
            options.AddAdditionalCapability("deviceName", "WindowsPC");
            options.AddAdditionalCapability("ms:experimental-webdriver", true);

            // Add newCommandTimeout if debugger is attached to allow debugging for more than 60 seconds.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                options.AddAdditionalCapability("newCommandTimeout", 3600);
            }

            // Start Appium clinet (and application under test)
            Driver = new WindowsDriver<WindowsElement>(service.ServiceUrl, options);
            Log.Info("Start Appium session to Windwos desktop.");

            // Set implicit timeout to auto wait for element when try to find it
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.Timeout);
        }

        public void Stop()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Log.Info("Stop Appium session to Windwos desktop.");
            }
        }
    }
}
