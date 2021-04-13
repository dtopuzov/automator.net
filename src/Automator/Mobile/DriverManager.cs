using System;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;

namespace Automator.Mobile
{
    /// <summary>
    /// Setup appium driver.
    /// </summary>
    internal class DriverManager
    {
        private readonly MobileSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverManager"/> class.
        /// </summary>
        /// <param name="settings">Instance of <see cref="MobileSettings"/>.</param>
        /// <param name="appiumServerUri">Uri of Appium server as <see cref="Uri"/>.</param>
        internal DriverManager(MobileSettings settings, Uri appiumServerUri)
        {
            this.settings = settings;
            var capabilities = GetCapabilities(settings);
            Driver = new RemoteWebDriver(appiumServerUri, capabilities);
        }

        internal RemoteWebDriver Driver { get; private set; }

        private static AppiumOptions GetCapabilities(MobileSettings settings)
        {
            var capabilities = new AppiumOptions();
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, settings.Platform);
            capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, settings.DeviceName);
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, settings.PlatformVersion);
            capabilities.AddAdditionalCapability(MobileCapabilityType.App, settings.App);

            if (settings.DeviceId != null)
            {
                capabilities.AddAdditionalCapability(MobileCapabilityType.Udid, settings.DeviceId);
            }

            if (settings.Avd != null)
            {
                capabilities.AddAdditionalCapability(AndroidMobileCapabilityType.Avd, settings.Avd);
            }

            if (settings.AvdArgs != null)
            {
                capabilities.AddAdditionalCapability(AndroidMobileCapabilityType.AvdArgs, settings.AvdArgs);
            }

            return capabilities;
        }
    }
}
