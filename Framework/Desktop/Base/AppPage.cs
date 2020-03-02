using System.Drawing;
using System.IO;
using Framework.Utils;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace Framework.Desktop
{
    /// <summary>
    /// Base class that represents web page.
    /// </summary>
    public abstract class AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppPage"/> class.
        ///
        /// It contains:
        /// Log -> To Log messages.
        /// Context -> Web Test Context (it includes Browser and Setttings objects).
        /// Wait -> WebDriverWait instance.
        ///
        /// Constructor will init all page-object elements on the page.
        /// </summary>
        /// <param name="context">Instance of AppPage.</param>
        protected AppPage(Context context)
        {
            Log = LogManager.GetLogger(GetType());
            Context = context;
        }

        protected ILog Log { get; set; }

        protected Context Context { get; set; }

        /// <summary>
        /// Compare current page with expected image.
        /// </summary>
        /// <param name="image">Name of expected image (without extension).</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="tolerance">Comparison tolerance as percent.</param>
        public void Match(string image, int timeout = 10, double tolerance = 0.01)
        {
            var path = Path.Combine(Context.Settings.ImagePath, string.Format("{0}.png", image));
            var result = ImageUtils.Compare(() => GetScreenshot(), path: path, timeout: timeout, tolerance: tolerance);
            if (result)
            {
                Log.Info(string.Format("App screen matches {0}", image));
            }

            Assert.IsTrue(result, string.Format("App screen does not match {0}", image));
        }

        /// <summary>
        /// Compare current page with expected image.
        /// </summary>
        /// <param name="element">WindowsElement.</param>
        /// <param name="image">Name of expected image (without extension).</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="tolerance">Comparison tolerance as percent.</param>
        public void MatchElement(WindowsElement element, string image, int timeout = 10, double tolerance = 0.01)
        {
            var path = Path.Combine(Context.Settings.ImagePath, string.Format("{0}.png", image));
            var result = ImageUtils.Compare(() => element.Screenshot(), path: path, timeout: timeout, tolerance: tolerance);
            if (result)
            {
                Log.Info(string.Format("Element matches {0}", image));
            }

            Assert.IsTrue(result, string.Format("Element does not match {0}", image));
        }

        /// <summary>
        /// Get screenshot of page.
        /// </summary>
        /// <returns>Bitmap image of the page.</returns>
        private Bitmap GetScreenshot()
        {
            Screenshot sc = Context.App.Driver.GetScreenshot();
            return Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
        }
    }
}