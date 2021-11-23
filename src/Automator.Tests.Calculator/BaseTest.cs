using NUnit.Framework;

namespace Automator.Tests.Calculator
{
    [TestFixture]
    public class BaseTest
    {
        private static App app;

        public static Calc Calc { get; private set; }

        [OneTimeSetUp]
        public void ClassInit()
        {
            app = new App(new Settings(), Setup.Server.Service.ServiceUrl);
            Calc = new Calc(app);
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
            app.Stop();
        }
    }
}
