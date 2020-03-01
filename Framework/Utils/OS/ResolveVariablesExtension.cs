using System;

namespace Framework.Utils
{
    public static class ResolveVariablesExtension
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
    }
}
