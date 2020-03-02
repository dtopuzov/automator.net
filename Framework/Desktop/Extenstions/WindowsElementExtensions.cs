using System.Drawing;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

namespace Framework.Desktop
{
    public static class WindowsElementExtensions
    {
        public static void DoubleClick(this WindowsElement element, int offsetX = 0, int offsetY = 0)
        {
            Actions actions = new Actions(element.WrappedDriver);
            actions.Build();
            actions.MoveToElement(element);
            actions.MoveByOffset(offsetX, offsetY);
            actions.DoubleClick();
            actions.Perform();
        }

        public static void Click(this WindowsElement element, int offsetX = 0, int offsetY = 0)
        {
            Actions actions = new Actions(element.WrappedDriver);
            actions.Build();
            actions.MoveToElement(element);
            actions.MoveByOffset(offsetX, offsetY);
            actions.Click();
            actions.Perform();
        }

        public static Bitmap Screenshot(this WindowsElement element)
        {
            var driver = element.WrappedDriver;
            Screenshot screenshot_full_screen = ((ITakesScreenshot)driver).GetScreenshot();
            Rectangle crop_rect = new Rectangle(element.Location, element.Size);
            Bitmap image_full_screen = Image.FromStream(new System.IO.MemoryStream(screenshot_full_screen.AsByteArray)) as Bitmap;
            _ = new Bitmap(image_full_screen);
            Bitmap image_element = image_full_screen.Clone(crop_rect, image_full_screen.PixelFormat);
            return image_element;
        }
    }
}