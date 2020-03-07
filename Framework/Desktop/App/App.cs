using System.Drawing;
using System.Reflection;
using Framework.Appium;
using log4net;
using OpenQA.Selenium.Appium.Windows;

namespace Framework.Desktop
{
    public class App
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly AppClient client;
        private readonly DesktopClient desktop;
        private readonly AppiumServer server;

        public App(Settings settings)
        {
            server = new AppiumServer();
            client = new AppClient(server.Service, settings);
            desktop = new DesktopClient(server.Service, settings);
        }

        public WindowsDriver<WindowsElement> Driver { get; protected set; }

        public WindowsDriver<WindowsElement> SystemDriver { get; protected set; }

        public string Title => Driver.Title;

        public bool IsRunning => Driver.WindowHandles.Count > 0;

        public void InitSession()
        {
            server.Start();

            // Start session to desktop
            desktop.Start();
            SystemDriver = desktop.Driver;

            // Start session to app under test
            client.Start();
            Driver = client.Driver;
        }

        public void QuitSession()
        {
            desktop.Stop();
            client.Stop();
            server.Stop();
        }

        public void CloseApp()
        {
            Driver.CloseApp();
            Log.Info("Close application.");
        }

        public void LaunchApp()
        {
            Driver.LaunchApp();
            Log.Info("Launch application.");
        }

        public void Restart()
        {
            if (IsRunning)
            {
                Driver.CloseApp();
            }

            Driver.LaunchApp();
            Log.Info("Restart application.");
        }

        public void SetSize(int width, int height)
        {
            Driver.Manage().Window.Size = new Size(width, height);
            Log.Info(string.Format("Set window size to {0}x{1}", width, height));
        }

        public void SetSize(Size size)
        {
            Driver.Manage().Window.Size = size;
            Log.Info(string.Format("Set window size to {0}x{1}", size.Width, size.Height));
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
