using NUnit.Framework;
using OpenQA.Selenium.Appium;

namespace Automator.Tests.Mobile.Tests
{
    public class LoginTests : BaseTest
    {
        [SetUp]
        public void BeforeLoginTest()
        {
            App.Click(MobileBy.AccessibilityId("Login"));
        }

        [Test]
        public void LoginWithValidUser()
        {
            Login("dtopuzov@gmail.com", "12345678");

            var alertLocator = MobileBy.Id("android:id/alertTitle");
            Assert.True(App.Find(alertLocator).Displayed);
        }

        [Test]
        public void LoginWithInvalidEmail()
        {
            Login("dtopuzov", "12345678");

            var message = App.FindByText("Please enter a valid email address");
            Assert.True(message.Displayed);
        }

        [Test]
        public void LoginWithShortPassword()
        {
            Login("dtopuzov@gmail.com", "123");

            var message = App.FindByText("Please enter at least 8 characters");
            Assert.True(message.Displayed);
        }

        private void Login(string user, string password)
        {
            App.Type(MobileBy.AccessibilityId("input-email"), user);
            App.Type(MobileBy.AccessibilityId("input-password"), password);
            App.Click(MobileBy.AccessibilityId("button-LOGIN"));
        }
    }
}
