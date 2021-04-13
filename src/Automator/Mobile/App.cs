using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using Automator.Shared.VisualTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace Automator.Mobile
{
    /// <summary>
    /// Browser abstraction.
    /// </summary>
    public class App
    {
        private readonly MobileSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="settings">Instance of <see cref="MobileSettings"/>.</param>
        /// <param name="appiumServerUri">Uri of Appium server as <see cref="Uri"/>.</param>
        public App(MobileSettings settings, Uri appiumServerUri)
        {
            this.settings = settings;
            Driver = new DriverManager(settings, appiumServerUri).Driver;
            Wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, TimeSpan.FromSeconds(this.settings.Timeout));
            Wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            Wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            Wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
        }

        /// <summary>
        /// Gets <see cref="IWebDriver"/> instance associated with current browser.
        /// </summary>
        public IWebDriver Driver { get; private set; }

        /// <summary>
        /// Gets <see cref="WebDriverWait"/> instance associated with current browser.
        /// </summary>
        public OpenQA.Selenium.Support.UI.WebDriverWait Wait { get; private set; }

        /// <summary>
        /// Gets or sets browser size.
        /// </summary>
        public Size Size
        {
            get => Driver.Manage().Window.Size;
            set => Driver.Manage().Window.Size = value;
        }

        /// <summary>
        /// Stop currently running browser.
        /// </summary>
        public void Stop()
        {
            Driver.Quit();
        }

        /// <summary>
        /// Find element by given locator.
        /// </summary>
        /// <param name="locator"><see cref="By"/> locator.</param>
        /// <param name="waitForExistence">If true it will wait until some results are found, otherwise will just return current state without any wait.</param>
        /// <returns>Instance of <see cref="IWebElement"/> when it exists, otherwise null.</returns>
        public IWebElement Find(By locator, bool waitForExistence = true)
        {
            try
            {
                if (waitForExistence)
                {
                    return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
                }
                else
                {
                    return Driver.FindElement(locator);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find elements by given locator.
        /// </summary>
        /// <param name="locator"><see cref="By"/> locator.</param>
        /// <param name="waitForExistence">If true it will wait until some results are found, otherwise will just return current state without any wait.</param>
        /// <returns><see cref="ReadOnlyCollection"/> of elements when found, otherwise null.</returns>
        public ReadOnlyCollection<IWebElement> FindAll(By locator, bool waitForExistence = true)
        {
            try
            {
                if (waitForExistence)
                {
                    return Wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
                }
                else
                {
                    return Driver.FindElements(locator);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Click on element.
        /// </summary>
        /// <param name="locator"><see cref="By"/> locator.</param>
        public void Click(By locator)
        {
            var element = Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            new Actions(Driver).MoveToElement(element).Click().Build().Perform();
        }

        /// <summary>
        /// Click on element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        public void Click(IWebElement element)
        {
            Wait.Until(ExpectedConditions.ElementToBeClickable(element));
            new Actions(Driver).MoveToElement(element).Click().Build().Perform();
        }

        /// <summary>
        /// Type text in element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <param name="text">Text to be typed.</param>
        /// <param name="clearBeforeType">If true clear content before type.</param>
        public void Type(IWebElement element, string text, bool clearBeforeType = true)
        {
            Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));

            if (clearBeforeType)
            {
                element.Clear();
            }

            element.SendKeys(text);
        }

        /// <summary>
        /// Type text in currently focused element.
        /// </summary>
        /// <param name="text">Text to be typed.</param>
        public void Type(string text)
        {
            new Actions(Driver).SendKeys(text).Build().Perform();
        }

        /// <summary>
        /// Take screenshot of the web page.
        /// </summary>
        /// <returns>Screenshot of the page as <see cref="Bitmap"/>.</returns>
        public Bitmap Screenshot()
        {
            Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            return ImageUtils.ConvertToBitmap(screenshot);
        }

        /// <summary>
        /// Compare current screen with expected image.
        /// </summary>
        /// <param name="image">Name of expected image (without extension).</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="tolerance">Comparison tolerance as percent.</param>
        /// <returns>Result of image comparison.</returns>
        public bool MatchScreen(string image, int timeout = 10, double tolerance = 0.01)
        {
            var path = Path.Combine(settings.VisualTesting.ImagePath, $"{image}.png");
            return ImageUtils.WaitForImageCompare(() => Screenshot(), path: path, timeout: timeout, tolerance: tolerance, settings.VisualTesting.ImageVerificationType);
        }
    }
}
