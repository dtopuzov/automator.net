using System.Reflection;
using log4net;
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
            Service.Start();
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
