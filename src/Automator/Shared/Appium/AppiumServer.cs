using System;
using Automator.Shared.Utils;
using OpenQA.Selenium.Appium.Service;

namespace Automator.Shared.Appium
{
    public class AppiumServer
    {
        public AppiumServer()
        {
            Service = new AppiumServiceBuilder().UsingAnyFreePort().Build();
        }

        public AppiumLocalService Service { get; }

        public void Start()
        {
            Service.Start();

            if (!Wait.Until(() => Service.IsRunning, timeout: 30))
            {
                throw new Exception("Failed to start Appium server!");
            }
        }

        public void Stop()
        {
            if (Service.IsRunning == true)
            {
                Service.Dispose();
            }
        }
    }
}
