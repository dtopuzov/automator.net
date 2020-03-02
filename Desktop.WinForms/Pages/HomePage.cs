using Framework.Desktop;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Desktop.WinForms.Pages
{
    public class HomePage : AppPage
    {
        public HomePage(Context context) : base(context) { }
        public void NavigateToSmallControls()
        {
            // Focus the app
            Context.App.Driver.FindElementByAccessibilityId("radPageView1").Click();

            // Keydown once
            Actions a = new Actions(Context.App.Driver);
            a.SendKeys(Keys.ArrowDown).Perform();
        }
    }
}
