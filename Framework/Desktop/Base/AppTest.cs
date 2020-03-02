using System.IO;
using System.Reflection;
using Framework.Utils;
using log4net;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Framework.Desktop
{
    public abstract class AppTest
    {
        private const string NodeProcessName = "node";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Context Context { get; set; }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            // Init Context
            Context = new Context
            {
                Settings = new Settings(),
            };

            // Ensure results folder exists
            FileSystem.CreateFolder(Context.Settings.TestResultsFolder);

            // Kill all application processes (and node processes too).
            KillProcesses();

            // Init application and start it
            Context.App = new App(Context.Settings);
            Context.App.InitSession();
        }

        [SetUp]
        public virtual void SetUp()
        {
            // Log beggining of test
            Log.Info("=======================================================");
            Log.Info("Start Test: " + TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public virtual void TearDown()
        {
            var test = TestContext.CurrentContext.Test.Name;
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            // Log test outcome
            Log.Info("Test End: " + test);
            Log.Info("Outcome: " + status.ToString());

            // Collect artefacts on test fail
            if (status == TestStatus.Failed)
            {
                CollectTestArtefacts(test);
            }
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            // Force kill app and node processes.
            // Notes: Call Driver.Quit() may cause some save dialogs that might affect testing.
            KillProcesses();
        }

        private void CollectTestArtefacts(string testName)
        {
            try
            {
                // Ensure results folder exists
                FileSystem.CreateFolder(Context.Settings.TestResultsFolder);

                // Save actual screenshot of desktop
                var baseName = FileSystem.NormalizePath(testName);
                var failedScreenPath = Path.Combine(Context.Settings.TestResultsFolder, baseName + "_desktop.png");
                ImageUtils.SaveScreenshot(failedScreenPath);

                // Save page source as XML
                if (Context.App.Driver != null)
                {
                    var pageSourceFile = baseName + ".xml";
                    var pageSourcePath = Path.Combine(Context.Settings.TestResultsFolder, pageSourceFile);
                    File.WriteAllText(pageSourcePath, Context.App.Driver.PageSource);
                }
            }
            catch
            {
                Log.Error("Failed to collect results on failed test: " + testName);
            }
        }

        /// <summary>
        /// Kill all Node and App processes.
        /// </summary>
        private void KillProcesses()
        {
            Process.KillProcessByName(NodeProcessName);
            Log.Info("Kill all node processes.");

            if (Context.Settings.AppProcess != null)
            {
                Process.KillProcessByName(Context.Settings.AppProcess);
                Log.Info("Kill all node processes.");
            }
        }
    }
}
