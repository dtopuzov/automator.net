using System.Drawing;
using Automator.Shared.VisualTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Automator.Web.Extensions
{
    /// <summary>
    /// IWebElement extensions.
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Get text of the element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <returns>Text of the element.</returns>
        public static string Text(this IWebElement element)
        {
            // Hack to avoid stale element exceptions.
            // In case of such exception we return null.
            try
            {
                return element.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get inner text of the element.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <returns>Inner test of the element.</returns>
        public static string InnerText(this IWebElement element)
        {
            try
            {
                return element.GetAttribute("innerText").Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Determine if element has the focus.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <returns>True if element is focused.</returns>
        public static bool HasFocus(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            return driver.SwitchTo().ActiveElement().Equals(element);
        }

        /// <summary>
        /// Determine if element is really visible in viewport.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <returns>True if visible.</returns>
        public static bool IsVisible(this IWebElement element)
        {
            if (element == null)
            {
                return false;
            }
            else
            {
                var driver = ((IWrapsDriver)element).WrappedDriver;
                var scriptExecutorDriver = (IJavaScriptExecutor)driver;

                var script = "var elem = arguments[0], " +
                    "box = elem.getBoundingClientRect()," +
                    "cx = box.left + box.width / 2," +
                    "cy = box.top + box.height / 2," +
                    "e = document.elementFromPoint(cx, cy);" +
                    "for (; e; e = e.parentElement) {" +
                    "  if (e === elem) " +
                    "    return true;" +
                    "}" +
                    "return false;";

                var result = scriptExecutorDriver.ExecuteScript(script, element);
                return bool.Parse(result.ToString());
            }
        }

        /// <summary>
        /// Scroll to element until it is in viewport.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        public static void ScrollTo(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var scriptExecutorDriver = (IJavaScriptExecutor)driver;
            scriptExecutorDriver.ExecuteScript("arguments[0].scrollIntoView();", element);
            System.Threading.Thread.Sleep(50);
            if (driver.GetType().Name.ToLower().Contains("firefox"))
            {
                System.Threading.Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Take screenshot of element.
        /// Note: Element must be in viewport, otherwise exception is trown.
        /// </summary>
        /// <param name="element">Instance of <see cref="IWebElement"/>.</param>
        /// <returns>Screenshot as <see cref="Bitmap"/>.</returns>
        public static Bitmap Screenshot(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var scriptExecutorDriver = (IJavaScriptExecutor)driver;
            var yOffset = int.Parse(scriptExecutorDriver.ExecuteScript("return window.pageYOffset;").ToString());
            var viewPortHeight = int.Parse(scriptExecutorDriver.ExecuteScript("return document.documentElement.clientHeight;").ToString());
            Screenshot screenshot_full_screen = ((ITakesScreenshot)driver).GetScreenshot();

            var x = element.Location.X;
            var y = element.Location.Y - yOffset;
            var width = element.Size.Width;
            var height = element.Size.Height;
            if (height + y > viewPortHeight)
            {
                height = viewPortHeight - y;
            }

            Rectangle crop_rect = new Rectangle(x, y, width, height);
            Bitmap image_full_screen = ImageUtils.ConvertToBitmap(screenshot_full_screen);
            _ = new Bitmap(image_full_screen);
            Bitmap image_element = image_full_screen.Clone(crop_rect, image_full_screen.PixelFormat);
            return image_element;
        }
    }
}
