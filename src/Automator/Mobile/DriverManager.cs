using System;
using System.Diagnostics;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace Automator.Mobile
{
    /// <summary>
    /// Setup appium driver.
    /// </summary>
    internal class DriverManager
    {
        private readonly AppiumDriver<AppiumWebElement> driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverManager"/> class.
        /// </summary>
        /// <param name="settings">Instance of <see cref="MobileSettings"/>.</param>
        /// <param name="appiumServerUri">Uri of Appium server as <see cref="Uri"/>.</param>
        internal DriverManager(MobileSettings settings, Uri appiumServerUri)
        {
            var capabilities = GetCapabilities(settings);
            driver = ((object)settings.Platform) switch
            {
                MobilePlatform.Android => new AndroidDriver<AppiumWebElement>(appiumServerUri, capabilities),
                MobilePlatform.IOS => new IOSDriver<AppiumWebElement>(appiumServerUri, capabilities),
                _ => throw new InvalidOperationException($"{settings.Platform} is invalid mobile platform."),
            };
        }

        internal AppiumDriver<AppiumWebElement> Driver => driver;

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

            if (Debugger.IsAttached)
            {
                capabilities.AddAdditionalCapability(MobileCapabilityType.NewCommandTimeout, "3600");
            }
            else
            {
                capabilities.AddAdditionalCapability(MobileCapabilityType.NewCommandTimeout, "180");
            }

            return capabilities;
        }
    }
}
