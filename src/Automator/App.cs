using System.Collections.ObjectModel;
using System.Drawing;
using Automator.Visual;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Interactions;

namespace Automator
{
    public class App
    {
        private readonly Settings settings;

        public App(Settings settings, Uri serverUri)
        {
            this.settings = settings;
            Driver = DriverManager.Init(settings, serverUri);
        }

        public AppiumDriver Driver { get; private set; }

        public void Stop()
        {
            if (Driver != null)
            {
                Driver.Quit();
            }
        }

        public AppiumElement Find(By locator)
        {
            Wait.Until(() => Driver.FindElements(locator).Count > 0);
            return Driver.FindElement(locator);
        }

        public AppiumElement FindByText(string text, bool exactMatch = false)
        {
            return Find(GetTextLocator(text, exactMatch));
        }

        public ReadOnlyCollection<AppiumElement> FindAll(By locator, bool wait = true)
        {
            try
            {
                if (wait)
                {
                    Wait.Until(() => Driver.FindElements(locator).Count > 0);
                }

                return Driver.FindElements(locator);
            }
            catch (InvalidOperationException)
            {
                return new List<AppiumElement>().AsReadOnly();
            }
        }

        public ReadOnlyCollection<AppiumElement> FindAllByText(string text, bool exactMatch = true, bool wait = true)
        {
            return FindAll(GetTextLocator(text, exactMatch), wait);
        }

        public void Click(By locator)
        {
            Find(locator).Click();
        }

        public void ContextClick(By locator)
        {
            var element = Find(locator);
            new Actions(Driver).ContextClick(element).Build().Perform();
        }

        public void Hover(By locator)
        {
            var element = Find(locator);
            new Actions(Driver).MoveToElement(element).Build().Perform();
        }

        public void Type(By locator, string text)
        {
            var element = Find(locator);
            element.SendKeys(text);
        }

        public void HideKeyboard()
        {
            Driver.HideKeyboard();
        }

        public Bitmap TakeScreenshot()
        {
            var windowLocator = MobileBy.IosClassChain("**/XCUIElementTypeWindow");
            Screenshot screenshot = settings.Device.PlatformType.Equals(MobilePlatform.MacOS)
                    ? Find(windowLocator).GetScreenshot()
                    : ((ITakesScreenshot)Driver).GetScreenshot();

            return Image.FromStream(new MemoryStream(screenshot.AsByteArray)) as Bitmap;
        }

        public bool CompareScreen(string image, double tolerance = 0.1)
        {
            var path = Path.Combine(settings.Visual.ImagePath, $"{image}.png");
            return ImageUtils.Compare(() => TakeScreenshot(), path: path, timeout: settings.General.Timeout, tolerance: tolerance);
        }

        private By GetTextLocator(string text, bool exactMatch = true)
        {
            var platform = settings.Device.PlatformType;
            if (platform.Equals(MobilePlatform.MacOS) || platform.Equals(MobilePlatform.IOS))
            {
                return exactMatch
                    ? MobileBy.IosNSPredicate($"value == \"{text}\" OR label == \"{text}\"")
                    : MobileBy.IosNSPredicate($"value contains '{text}' OR label contains '{text}'");
            }
            else if (platform.Equals(MobilePlatform.Android))
            {
                return exactMatch
                    ? MobileBy.AndroidUIAutomator($"new UiSelector().text(\"{text}\")")
                    : MobileBy.AndroidUIAutomator($"new UiSelector().textContains(\"{text}\")");
            }
            else
            {
                return exactMatch
                    ? By.XPath($"//*[@text='{text}']")
                    : By.XPath($"//*[contains(@text,'{text}')]");
            }
        }
    }
}
