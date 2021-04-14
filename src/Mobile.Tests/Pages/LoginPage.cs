using Automator.Mobile;
using OpenQA.Selenium.Appium;

namespace Mobile.Tests.Pages
{
    public class LoginPage
    {
        private App app;

        public LoginPage(App app)
        {
            this.app = app;
        }

        public void NavigateTo()
        {
            app.Find(MobileBy.AccessibilityId("Login")).Click();
        }

        public void Login(string user, string password)
        {
            var inputUser = app.Find(MobileBy.AccessibilityId("input-email"));
            inputUser.Clear();
            inputUser.SendKeys(user);

            var inputPass = app.Find(MobileBy.AccessibilityId("input-password"));
            inputPass.Clear();
            inputPass.SendKeys(password);

            app.Find(MobileBy.AccessibilityId("button-LOGIN")).Click();
        }
    }
}
