using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Automator
{
    internal class Config
    {
        public static IConfiguration GetDefaultConfiguration()
        {
            var testConfig = Environment.GetEnvironmentVariable("CONFIG");
            if (testConfig == null)
            {
                LoadEnvironmentVariablesFromProjectSettings();
                testConfig = Environment.GetEnvironmentVariable("CONFIG");
            }

            return GetConfigurationFromFile($"appsettings.{testConfig}.json");
        }

        public static IConfiguration GetConfigurationFromFile(string config)
        {
            return new ConfigurationBuilder()
                .AddJsonFile(config, optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        public static T GetSetting<T>(string variableName, T defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(variableName);
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        private static void LoadEnvironmentVariablesFromProjectSettings()
        {
            string launchSettingsFile = $"{AppDomain.CurrentDomain.BaseDirectory}/Properties/launchSettings.json";
            if (!File.Exists(launchSettingsFile))
            {
                return;
            }
            else
            {
                var launchSettings = File.ReadAllText(launchSettingsFile);
                var settingsObject = JObject.Parse(launchSettings);
                var variables = settingsObject
                    .GetValue("profiles")
                     .SelectMany(profiles => profiles.Children())
                     .SelectMany(profile => profile.Children<JProperty>())
                     .Where(prop => prop.Name == "environmentVariables")
                     .SelectMany(prop => prop.Value.Children<JProperty>())
                     .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }
    }
}
