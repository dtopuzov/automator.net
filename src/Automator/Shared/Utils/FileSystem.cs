using System;
using System.IO;
using System.Linq;

namespace Automator.Shared.Utils
{
    /// <summary>
    /// File system utils.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Gets path to current project.
        /// </summary>
        public static string ProjectRoot
        {
            get
            {
                var binDebugFolder = Environment.CurrentDirectory;
                var projectRoot = new DirectoryInfo(binDebugFolder);
                while (projectRoot != null && !projectRoot.GetFiles("*.csproj").Any())
                {
                    projectRoot = projectRoot.Parent;
                }

                return projectRoot.FullName;
            }
        }

        /// <summary>
        /// Resolve windows environment variables inside string.
        /// </summary>
        /// <param name="text">Input string.</param>
        /// <returns>String with resolved environment variables.</returns>
        public static string ResolveVariables(this string text)
        {
            if (text.Contains('%'))
            {
                string variable = text.Split('%', '%')[1];
                return text.Replace($"%{variable}%", Environment.GetEnvironmentVariable(variable));
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// Remove all invalid PATH symbols of given string.
        /// </summary>
        /// <param name="path">String.</param>
        /// <returns>String with escaped invalid PATH symbols.</returns>
        public static string NormalizePath(this string path)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), string.Empty);
            }

            return path;
        }
    }
}
