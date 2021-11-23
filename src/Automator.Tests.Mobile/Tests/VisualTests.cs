using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace Automator.Tests.Mobile.Tests
{
    public class VisualTests : BaseTest
    {
        [Test]
        public void HomeShouldLookOK()
        {
            App.Click(MobileBy.AccessibilityId("Home"));

            var looksOK = App.CompareScreen("home-page", tolerance: 0.5);
            Assert.True(looksOK, "Home page does not look ok.");
        }

        [Test]
        public void LoginShouldLookOK()
        {
            App.Click(MobileBy.AccessibilityId("Login"));

            var looksOK = App.CompareScreen("login-page", tolerance: 0.5);
            Assert.True(looksOK, "Login page does not look ok.");
        }
    }
}
