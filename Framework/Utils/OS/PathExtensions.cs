using System;
using System.IO;

namespace Framework.Utils
{
    public static class PathExtensions
    {
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
                return text.Replace("%" + variable + "%", Environment.GetEnvironmentVariable(variable));
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
