using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading;
using Automator.Shared.VisualTesting;
using Automator.Web.Enums;
using Automator.Web.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Automator.Web
{
    /// <summary>
    /// Browser abstraction.
    /// </summary>
    public class Browser
    {
        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser"/> class.
        /// </summary>
        /// <param name="settings">Instance of <see cref="Settings"/>.</param>
        public Browser(Settings settings)
        {
            this.settings = settings;
            Driver = new DriverManager(settings).Driver;
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(this.settings.Timeout))
            {
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };
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
        public WebDriverWait Wait { get; private set; }

        /// <summary>
        /// Gets or sets browser size.
        /// </summary>
        public Size Size
        {
            get => Driver.Manage().Window.Size;
            set => Driver.Manage().Window.Size = value;
        }

        /// <summary>
        /// Gets the size of view port.
        /// </summary>
        public Size ViewPortSize
        {
            get
            {
                var width = ExecuteScript("return document.documentElement.clientWidth;");
                var height = ExecuteScript("return document.documentElement.clientHeight;");
                return new Size(int.Parse(width), int.Parse(height));
            }
        }

        /// <summary>
        /// Stop currently running browser.
        /// </summary>
        public void Stop()
        {
            Driver.Quit();
        }

        /// <summary>
        /// Get browser logs.
        /// </summary>
        /// <returns>Logs as <see cref="ReadOnlyCollection"/>.</returns>
        public ReadOnlyCollection<LogEntry> GetLogs()
        {
            return Driver.Manage().Logs.GetLog(LogType.Browser);
        }

        /// <summary>
        /// Navigate to given URL.
        /// </summary>
        /// <param name="url">URL as <see cref="string"/>.</param>
        /// <param name="forceRefresh">If true opens the URL. If false and already on the same URL it will do nothing.</param>
        public void NavigateTo(string url, bool forceRefresh = true)
        {
            if (forceRefresh || !Driver.Url.Equals(url, StringComparison.OrdinalIgnoreCase))
            {
                Driver.Navigate().GoToUrl(url);
            }

            Wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
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
            if (settings.BrowserType == BrowserType.Firefox)
            {
                if (!element.IsVisible())
                {
                    element.ScrollTo();
                }
            }

            Wait.Until(ExpectedConditions.ElementToBeClickable(element));
            new Actions(Driver).MoveToElement(element).Click().Build().Perform();
        }

        /// <summary>
        /// Hover over element.
        /// </summary>
        /// <param name="locator"><see cref="By"/> locator.</param>
        public void Hover(By locator)
        {
            var element = Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            new Actions(Driver).MoveToElement(element).Build().Perform();
        }

        /// <summary>
        /// Hover over element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        public void Hover(IWebElement element)
        {
            new Actions(Driver).MoveToElement(element).Build().Perform();
        }

        /// <summary>
        /// Context (right) click over element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        public void ContextClick(IWebElement element)
        {
            new Actions(Driver).ContextClick(element).Build().Perform();
        }

        /// <summary>
        /// Double click over element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        public void DoubleClick(IWebElement element)
        {
            new Actions(Driver).DoubleClick(element).Build().Perform();
        }

        /// <summary>
        /// Type text in element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <param name="text">Text to be typed.</param>
        /// <param name="clearBeforeType">If true clear content before type.</param>
        /// <param name="sendEnterKey">If true send Enter key after typing text.</param>
        public void Type(IWebElement element, string text, bool clearBeforeType = true, bool sendEnterKey = true)
        {
            Wait.Until(ExpectedConditions.ElementToBeClickable(element));

            // Hack for FF to increase stability
            if (settings.BrowserType == BrowserType.Firefox && !element.HasFocus())
            {
                element.Click();
                Thread.Sleep(100);
            }

            if (clearBeforeType)
            {
                element.Clear();
            }

            element.SendKeys(text);

            if (sendEnterKey)
            {
                element.SendKeys(Keys.Return);
            }
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
        /// Type text in currently focused element.
        /// </summary>
        /// <param name="key1">First key.</param>
        /// <param name="key2">Second key.</param>
        public void SendKeyCombination(string key1, string key2)
        {
            new Actions(Driver).KeyDown(key1).KeyDown(key2).KeyUp(key2).KeyUp(key1).Build().Perform();
        }

        /// <summary>
        /// Execute JavaScript in browser.
        /// </summary>
        /// <param name="script">Script as <see cref="string"/>.</param>
        /// <returns>Result as <see cref="string"/>.</returns>
        public string ExecuteScript(string script)
        {
            try
            {
                return ((IJavaScriptExecutor)Driver).ExecuteScript(script).ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Execute JavaScript in element.
        /// </summary>
        /// <param name="script">Script as <see cref="string"/>.</param>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <returns>Result as <see cref="string"/>.</returns>
        public string ExecuteScript(string script, IWebElement element)
        {
            try
            {
                return ((IJavaScriptExecutor)Driver).ExecuteScript(script, element).ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Take screenshot of the web page.
        /// </summary>
        /// <returns>Screenshot of the page as <see cref="Bitmap"/>.</returns>
        public Bitmap Screenshot()
        {
            Screenshot screenshot_full_screen = ((ITakesScreenshot)Driver).GetScreenshot();
            return ImageUtils.ConvertToBitmap(screenshot_full_screen);
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

        /// <summary>
        /// Compare <see cref="IWebElement"/> with expected image.
        /// </summary>
        /// <param name="element">WindowsElement.</param>
        /// <param name="image">Name of expected image (without extension).</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="tolerance">Comparison tolerance as percent.</param>
        /// <returns>Result of image comparison.</returns>
        public bool MatchElement(IWebElement element, string image, int timeout = 10, double tolerance = 0.01)
        {
            var path = Path.Combine(settings.VisualTesting.ImagePath, $"{image}.png");
            return ImageUtils.WaitForImageCompare(() => element.Screenshot(), path: path, timeout: timeout, tolerance: tolerance, settings.VisualTesting.ImageVerificationType);
        }
    }
}
