using System.Reflection;
using System.Threading;
using Framework.Utils;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Service;

namespace Framework.Appium
{
    public class AppiumServer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AppiumServer()
        {
            Service = new AppiumServiceBuilder().UsingAnyFreePort().Build();
        }

        public AppiumLocalService Service { get; }

        public void Start()
        {
            Process.KillProcessByName("node");
            Thread.Sleep(1000);
            Service.Start();
            var started = Wait.Until(() => Service.IsRunning, 30);
            Assert.IsTrue(started, "Failed to Start appium server.");
            Log.Info("Start Appium Server.");
        }

        public void Stop()
        {
            if (Service.IsRunning == true)
            {
                Service.Dispose();
            }

            Log.Info("Stop Appium Server.");
        }
    }
}
