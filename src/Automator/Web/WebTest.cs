using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Automator.Web
{
    public abstract class WebTest
    {
        public Browser Browser { get; private set; }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            var settings = new Settings();
            Browser = new Browser(settings);
        }

        [TearDown]
        public virtual void TearDown()
        {
            var test = TestContext.CurrentContext.Test.Name;
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            // Collect artefacts on test fail
            if (status == TestStatus.Failed)
            {
                CollectTestArtefacts(test);
            }
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Browser.Stop();
        }

        private void CollectTestArtefacts(string testName)
        {
        }
    }
}
