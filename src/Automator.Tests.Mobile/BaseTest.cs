using NUnit.Framework;

namespace Automator.Tests.Mobile
{
    [TestFixture]
    public class BaseTest
    {
        public static App App { get; private set; }

        [OneTimeSetUp]
        public void ClassInit()
        {
            App = Setup.App;
            App.Driver.ResetApp();
        }

        [SetUp]
        public void TestInit()
        {
            // Runs before each test. (Optional)
        }

        [TearDown]
        public void TestCleanup()
        {
            // Runs after each test. (Optional)
        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            // Runs once after all tests in this class are executed. (Optional)
            // Not guaranteed that it executes instantly after all tests from the class.
        }
    }
}
