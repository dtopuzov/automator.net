using System.Diagnostics;
using Automator.Utils;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Mac;
using OpenQA.Selenium.Appium.Windows;

namespace Automator
{
    public class DriverManager
    {
        public static AppiumDriver Init(Settings settings, Uri serverUri)
        {
            var capabilities = GetOptions(settings);

            return settings.Device.PlatformType.ToString() switch
            {
                MobilePlatform.Android => new AndroidDriver(serverUri, capabilities),
                MobilePlatform.IOS => new IOSDriver(serverUri, capabilities),
                MobilePlatform.Windows => new WindowsDriver(serverUri, capabilities),
                MobilePlatform.MacOS => new MacDriver(serverUri, capabilities),
                _ => throw new InvalidOperationException($"{settings.Device.PlatformType} is invalid mobile platform."),
            };
        }

        private static AppiumOptions GetOptions(Settings settings)
        {
            var options = new AppiumOptions();

            // Set Device Capabilities
            options.PlatformName = settings.Device.PlatformType.ToString();
            options.PlatformVersion = settings.Device.PlatformVersion;
            options.DeviceName = settings.Device.DeviceName;
            options.AutomationName = settings.Device.PlatformType.ToString() switch
            {
                MobilePlatform.Android => AutomationName.AndroidUIAutomator2,
                MobilePlatform.IOS => AutomationName.iOSXcuiTest,
                MobilePlatform.Windows => "Windows",
                MobilePlatform.MacOS => "Mac2",
                _ => throw new InvalidOperationException($"{settings.Device.PlatformType} is invalid mobile platform."),
            };

            if (settings.Device.DeviceType == DeviceType.Emulator)
            {
                options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.Avd, settings.Device.DeviceName);
            }

            if (settings.Device.DeviceId != null)
            {
                options.AddAdditionalAppiumOption(MobileCapabilityType.Udid, settings.Device.DeviceId);
            }

            // Enable parallelism for Android
            if (options.AutomationName == AutomationName.AndroidUIAutomator2)
            {
                options.AddAdditionalAppiumOption("systemPort", Network.GetAvailablePort(8200, 8300));
            }

            // Enable parallelism for iOS
            if (options.AutomationName == AutomationName.iOSXcuiTest)
            {
                options.AddAdditionalAppiumOption("wdaLocalPort", Network.GetAvailablePort(8100, 8199));
            }

            // Set App Capabilities
            if (settings.Device.PlatformType.Equals(MobilePlatform.Android))
            {
                if (settings.App.Path != null)
                {
                    options.App = settings.App.Path;
                }
                else
                {
                    options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, settings.App.Activity);
                    options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, settings.App.Id);
                }

                if (settings.App.Activity != string.Empty)
                {
                    options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppWaitActivity, settings.App.Activity);
                    options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppWaitPackage, settings.App.Id);
                }
            }
            else if (settings.Device.PlatformType.Equals(MobilePlatform.IOS))
            {
                options.App = settings.App.Path;
            }
            else if (settings.Device.PlatformType.Equals(MobilePlatform.MacOS))
            {
                options.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, settings.App.Id);
            }
            else if (settings.Device.PlatformType.Equals(MobilePlatform.Windows))
            {
                if (settings.App.Path != null)
                {
                    options.App = settings.App.Path;
                }
                else if (settings.App.Id != null)
                {
                    options.App = settings.App.Id;
                }
                else
                {
                    throw new Exception("App.Path or App.Id must be set in config file!");
                }

                if (settings.App.Args != string.Empty)
                {
                    options.AddAdditionalAppiumOption("appArguments", settings.App.Args);
                }
            }
            else
            {
                throw new Exception($"Unknown platform: {settings.Device.PlatformType}");
            }

            // Set General Capabilities
            if (Debugger.IsAttached)
            {
                options.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, "3600");
            }
            else
            {
                options.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, "180");
            }

            return options;
        }
    }
}
