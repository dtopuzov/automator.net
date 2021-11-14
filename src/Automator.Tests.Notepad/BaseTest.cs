#pragma warning disable IDE0060 // Remove unused parameter
using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automator.Tests.Notepad
{
    [TestClass]
    public abstract class BaseTest
    {
        public TestContext TestContext { get; set; }

        protected static AppiumServer Server { get; set; }

        protected static App App { get; set; }

        protected static Settings Settings { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Server = new AppiumServer();
            Server.Start();

            var binDebug = $"bin{Path.DirectorySeparatorChar}Debug";
            var projectRoot = AppDomain.CurrentDomain.BaseDirectory.Split(binDebug)[0];

            Settings = new Settings();
            Settings.App.Args = Path.Combine(projectRoot, "Data\\notes.txt");

            App = new App(Settings, Server.Service.ServiceUrl);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            App.Stop();
        }

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInitialize(TestContext context)
        {
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var testName = $"{TestContext.FullyQualifiedTestClassName.Split('.').Last()}.{TestContext.TestName}";
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                CollectTestArtefacts(testName);
            }
        }

        private void CollectTestArtefacts(string testName)
        {
        }
    }
}
