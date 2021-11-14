using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Mac;

namespace Automator.Appium
{
    public static class Extensions
    {
        public static bool IsChecked(this AppiumElement element)
        {
            var driver = element.WrappedDriver;
            if (driver is AndroidDriver)
            {
                return element.GetAttribute("checked").Equals("true");
            }
            else if (driver is IOSDriver || driver is MacDriver)
            {
                return element.GetAttribute("selected").Equals("true");
            }
            else
            {
                throw new NotImplementedException($"IsChecked not implemented for {driver.GetType()}");
            }
        }
    }
}
