using Automator.Mobile;
using Mobile.Tests.Pages;
using NUnit.Framework;

namespace Mobile.Tests.Tests.WDemo
{
    public class LoginTests : MobileTest
    {
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            App.Restart();
            loginPage = new LoginPage(App);
            loginPage.NavigateTo();
        }

        [Test]
        public void Login_With_ValidUser()
        {
            loginPage.Login("dtopuzov@gmail.com", "myValidPassword");
        }

        [Test]
        public void Login_With_ShortPassword()
        {
            loginPage.Login("dtopuzov@gmail.com", "short");
        }

        [Test]
        public void Login_With_InvalidEmail()
        {
            loginPage.Login("dtopuzov", "myValidPassword");
        }
    }
}
