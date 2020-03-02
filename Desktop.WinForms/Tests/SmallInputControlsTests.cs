using Desktop.WinForms.Pages;
using Framework.Desktop;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;

namespace Desktop.WinForms.Tests
{
    public class SmallInputControlsTests : AppTest
    {
        private HomePage HomePage { get; set; }

        [OneTimeSetUp]
        public void SmallInputControlsTestsOneTimeSetUp()
        {
            HomePage = new HomePage(Context);
            HomePage.NavigateToSmallControls();
        }

        [SetUp]
        public void SmallInputControlsTestsSetUp()
        {
            if (!Context.App.IsRunning)
            {
                Context.App.LaunchApp();
            }

            // Check if we are in default theme and rollback if not
            var theme = Context.App.Driver.FindElementByAccessibilityId("radDropDownButtonThemes");
            if (!theme.Text.Equals("ControlDefault"))
            {
                theme.Click();
                Context.App.SystemDriver.FindElementByName("ControlDefault").Click();
            }

        }

        [Test]
        public void ButtonsDropDown()
        {
            var dropDownButton = Context.App.Driver.FindElementByAccessibilityId("radDropDownButton1");

            // Select form drop down button
            dropDownButton.Click();
            Context.App.SystemDriver.FindElementByName("radMenuItem1").Click();

            // Select form drop down button
            dropDownButton.Click();
            Context.App.SystemDriver.FindElementByName("radMenuItem2").Click();

            // Verify context menu looks OK
            dropDownButton.Click();
            var dropDown = Context.App.SystemDriver.FindElementByName("DropDownDropDown");
            HomePage.MatchElement(dropDown, "dropdown_default");

            // Verify context menu looks OK in 
            Context.App.Driver.FindElementByAccessibilityId("radDropDownButtonThemes").Click();
            Context.App.SystemDriver.FindElementByName("VisualStudio2012Dark").Click();
            dropDownButton.Click();
            dropDown = Context.App.SystemDriver.FindElementByName("DropDownDropDown");
            HomePage.MatchElement(dropDown, "dropdown_vs2012dark");

        }

        [Test]
        public void Calculator()
        {
            var calcButton = Context.App.Driver.FindElementByAccessibilityId("radCalculator1");
            calcButton.Click((calcButton.Rect.Width / 2) - 5, 0);
            
            // Notes: 
            // - No good locator for popup
            // - A lot of movable parts, but good case to set tolerance 1 and show what happens when test fails.
            HomePage.Match("calculator_default", tolerance: 10);
        }
    }
}
