using OpenQA.Selenium.Appium;
using System.Drawing;

namespace Automator.Tests.Calculator
{
    public class Calc
    {
        private readonly App app;

        public Calc(App app)
        {
            this.app = app;
        }

        public string Result => app.Find(MobileBy.AccessibilityId("CalculatorResults")).GetAttribute("Name");

        public void PressDigit(int digit) => app.Click(MobileBy.AccessibilityId($"num{digit}Button"));

        public void PressMultiply() => app.Click(MobileBy.AccessibilityId("multiplyButton"));

        public void PressPlus() => app.Click(MobileBy.AccessibilityId("plusButton"));

        public void PressEqual() => app.Click(MobileBy.AccessibilityId("equalButton"));

        public void SetSize(int width, int height)
        {
            this.app.Driver.Manage().Window.Size = new Size(width, height);
        }
    }
}
