using System.Drawing;
using System.IO;
using System.Reflection;
using Framework.Appium;
using Framework.Utils;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;

namespace Framework.Desktop
{
    public class App
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly WindowsClient client;
        private readonly AppiumServer server;
        private Settings settings;

        public App(Settings settings)
        {
            this.settings = settings;
            server = new AppiumServer();
            client = new WindowsClient(server.Service, settings);
        }

        public WindowsDriver<WindowsElement> Driver { get; protected set; }

        public string Title => Driver.Title;

        public bool IsRunning
        {
            get
            {
                try
                {
                    _ = Title;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Start()
        {
            server.Start();
            client.Start();
            Driver = client.Driver;
        }

        public void Stop()
        {
            client.Stop();
            server.Stop();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void SetSize(int width, int height)
        {
            Driver.Manage().Window.Size = new Size(width, height);
            Log.Info(string.Format("Set window size to {0}x{1}", width, height));
        }

        public void SetPosition(int x, int y)
        {
            Driver.Manage().Window.Position = new Point(x, y);
            Log.Info(string.Format("Set window position to {0}x{1}", x, y));
        }

        public void Maximize()
        {
            Driver.Manage().Window.Maximize();
            Log.Info("Maximize application.");
        }
    }
}
