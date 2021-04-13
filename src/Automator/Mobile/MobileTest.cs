using Automator.Shared.Appium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Automator.Mobile
{
    public abstract class MobileTest
    {
        private AppiumServer server;

        public App App { get; private set; }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            var settings = new MobileSettings();

            server = new AppiumServer();
            server.Start();

            App = new App(settings, server.Service.ServiceUrl);
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
            App.Stop();
            server.Stop();
        }

        private void CollectTestArtefacts(string testName)
        {
        }
    }
}
