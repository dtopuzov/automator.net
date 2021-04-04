using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Automator.Shared.Utils
{
    /// <summary>
    /// Config utils to help generating settings for test run.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Get host OS type.
        /// </summary>
        /// <returns>Host OS type as <see cref="VisualTestingSettings"/>.</returns>
        public static OSPlatform GetOSType()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }
            else
            {
                throw new Exception("Unknown operating system detected!");
            }
        }

        /// <summary>
        /// Get environment variable.
        /// </summary>
        /// <typeparam name="T">Type of variable.</typeparam>
        /// <param name="name">Name of environment variable.</param>
        /// <param name="defaultValue">Default value (will be used if there is no such enty in App.config).</param>
        /// <returns>Retruns variable value (or default).</returns>
        public static T GetEnvironmentVariable<T>(string name, T defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(name);
            return value != null
                ? typeof(T).IsEnum
                    ? (T)Enum.Parse(typeof(T), value)
                    : (T)Convert.ChangeType(value, typeof(T))
                : defaultValue;
        }

        /// <summary>
        /// Get configuration from appsettings based on current environment.
        /// </summary>
        /// <returns>Instance of <see cref="IConfiguration"/>.</returns>
        public static IConfiguration GetConfiguration()
        {
            var testConfig = Environment.GetEnvironmentVariable("CONFIG");
            if (testConfig == null)
            {
                LoadEnvironmentVariablesFromProjectSettings();
                testConfig = Environment.GetEnvironmentVariable("CONFIG");
            }

            return new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{testConfig}.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Load variables from "Properties/launchSettings.json" file.
        ///
        /// Notes:
        /// - Visual Studio do not respect system environment variables.
        /// - For this purpose we get all "environmentVariables" from launchSettings.json and set them as system variables.
        /// </summary>
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
