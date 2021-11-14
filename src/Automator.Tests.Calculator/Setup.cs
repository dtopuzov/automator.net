using NUnit.Framework;

namespace Automator.Tests.Calculator
{
    /// <summary>
    /// A SetUpFixture outside of any namespace provides SetUp and TearDown for the entire assembly.
    /// </summary>
    [SetUpFixture]
    public class Setup
    {
        private AppiumServer server;

        public static App App { get; set; }

        public static Settings Settings { get; set; }

        /// <summary>
        /// Executes once before the test run.
        /// </summary>
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            server = new AppiumServer();
            server.Start();

            Settings = new Settings();
            App = new App(Settings, server.Service.ServiceUrl);
        }

        /// <summary>
        /// Executes once after the test run.
        /// </summary>
        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            App.Stop();
            server.Stop();
        }
    }
}
