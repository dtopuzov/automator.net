using System.Drawing;
using System.IO;
using System.Reflection;
using Framework.Utils;
using log4net;

namespace Framework.Desktop
{
    public class Settings
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            InitSettings();
            LogSettings();
        }

        public string AppPath { get; set; }

        public string AppProcess { get; set; }

        public string AppTitle { get; set; }

        public Size? Size { get; set; }

        public short Timeout { get; set; }

        public string ImagePath { get; set; }

        public string TestResultsFolder { get; set; }

        /// <summary>
        /// Init test settings from App.config.
        /// </summary>
        private void InitSettings()
        {
            AppPath = Config.GetEnvironmentVariable("APP_PATH", Config.Get<string>("app", "executable", null)).ResolveVariables();
            AppProcess = Config.GetEnvironmentVariable("APP_PROCESS", Config.Get<string>("app", "process", null));
            AppTitle = Config.GetEnvironmentVariable("APP_TITLE", Config.Get<string>("app", "title", null));
            Size = GetSize(Config.GetEnvironmentVariable("SIZE", Config.Get<string>("app", "size", null)));
            Timeout = Config.GetEnvironmentVariable("TIMEOUT", Config.Get<short>("general", "wait", 30));
            ImagePath = Config.GetEnvironmentVariable("IMAGE_PATH", Config.Get("general", "image_path", Path.Combine(FileSystem.ProjectRoot, "Images"))).ResolveVariables();
            TestResultsFolder = Path.Combine(FileSystem.ProjectRoot, "TestResults");
        }

        /// <summary>
        /// Log test settigns.
        /// </summary>
        private void LogSettings()
        {
            Log.Info("=======================================================");
            Log.Info("                   Test Settings                       ");
            Log.Info(string.Empty);
            Log.Info("Application Path: " + AppPath);
            Log.Info("Application Process Name: " + AppProcess);
            Log.Info("Application Title: " + AppTitle);
            Log.Info("Default Timeout: " + Timeout.ToString());
            Log.Info("Expected Image Folder: " + ImagePath);
            Log.Info("Test Results Folder: " + TestResultsFolder);
            Log.Info("=======================================================");
        }

        private Size? GetSize(string size)
        {
            if (size == null)
            {
                return null;
            }
            else
            {
                var list = size.Split('x');
                int width = int.Parse(list[0]);
                int height = int.Parse(list[1]);
                return new Size(width, height);
            }
        }
    }
}
