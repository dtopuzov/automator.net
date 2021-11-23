using NUnit.Framework;

namespace Automator.Tests.Calculator
{
    /// <summary>
    /// A SetUpFixture outside of any namespace provides SetUp and TearDown for the entire assembly.
    /// </summary>
    [SetUpFixture]
    public class Setup
    {
        public static AppiumServer Server { get; private set; }

        /// <summary>
        /// Executes once before the test run.
        /// </summary>
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Server = new AppiumServer();
            Server.Start();
        }

        /// <summary>
        /// Executes once after the test run.
        /// </summary>
        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            Server.Stop();
        }
    }
}
