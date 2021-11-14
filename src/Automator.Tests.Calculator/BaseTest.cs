using NUnit.Framework;

namespace Automator.Tests.Calculator
{
    [TestFixture]
    public class BaseTest
    {
        [OneTimeSetUp]
        public void ClassInit()
        {
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
