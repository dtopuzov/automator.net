using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

namespace Framework.Desktop
{
    public static class WindowsElementExtensions
    {
        public static void DoubleClick(this WindowsElement element, int offsetX = 10, int offsetY = 10)
        {
            Actions actions = new Actions(element.WrappedDriver);
            actions.Build();
            actions.MoveToElement(element);
            actions.DoubleClick();
            actions.Perform();
        }

        public static void Click(this WindowsElement element, int offsetX = 10, int offsetY = 10)
        {
            Actions actions = new Actions(element.WrappedDriver);
            actions.Build();
            actions.MoveToElement(element, offsetX, offsetY);
            actions.Click();
            actions.Perform();
        }

        public static Bitmap GetScreenshotAsBitmap(this WindowsElement element)
        {
            var driver = element.WrappedDriver;
            OpenQA.Selenium.Screenshot screenshot_full_screen = ((ITakesScreenshot)driver).GetScreenshot();
            Rectangle crop_rect = new Rectangle(element.Location, element.Size);
            Bitmap image_full_screen = Image.FromStream(new System.IO.MemoryStream(screenshot_full_screen.AsByteArray)) as Bitmap;
            Bitmap image_element = new Bitmap(image_full_screen);
            image_element = image_full_screen.Clone(crop_rect, image_full_screen.PixelFormat);
            return image_element;
        }
    }
}