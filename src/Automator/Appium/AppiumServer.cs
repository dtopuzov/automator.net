using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;

namespace Automator
{
    public class AppiumServer
    {
        public AppiumServer()
        {
            var options = new OptionCollector()
                .AddArguments(new KeyValuePair<string, string>("--plugins", "images"))
                .AddArguments(new KeyValuePair<string, string>("--base-path", "/wd/hub"));

            Service = new AppiumServiceBuilder()
                .WithArguments(options)
                .UsingAnyFreePort()
                .Build();
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
            if (Service != null && Service.IsRunning)
            {
                Service.Dispose();
            }
        }
    }
}
