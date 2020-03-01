using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Framework.Utils
{
    /// <summary>
    /// Config helps generating settings for test run.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Get setting from appsettings.json.
        /// </summary>
        /// <typeparam name="T">Type of setting.</typeparam>
        /// <param name="section">Section in appsettings.json (example: app, general).</param>
        /// <param name="setting">Name of entry in appsettings.json.</param>
        /// <param name="defaultValue">Default value (will be used if there is no such enty in App.config).</param>
        /// <returns>Value of setting (or default).</returns>
        public static T Get<T>(string section, string setting, T defaultValue)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            var config = builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();

            var sectionItem = config.GetSection(section);
            if (sectionItem == null)
            {
                return defaultValue;
            }
            else
            {
                var rowValue = sectionItem.GetSection(setting);
                if (rowValue.Value == null)
                {
                    return defaultValue;
                }
                else
                {
                    var value = rowValue.Value;
                    if (typeof(T).IsEnum)
                    {
                        return (T)Enum.Parse(typeof(T), value);
                    }

                    return (T)Convert.ChangeType(value, typeof(T));
                }
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
            if (value != null)
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
