using System.Diagnostics;

namespace Automator
{
    public class Wait
    {
        /// <summary>
        /// Wait intil function return true or timeout is reached.
        /// </summary>
        /// <param name="task">Bool function.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="retryInterval">Deplay between retries in milliseconds.</param>
        /// <returns>Result of bool function (false in case timeout reached).</returns>
        public static bool Until(Func<bool> task, double timeout = 10, int retryInterval = 100)
        {
            bool success = false;
            TimeSpan maxDuration = TimeSpan.FromSeconds(timeout);
            Stopwatch sw = Stopwatch.StartNew();
            while (success != true && (sw.Elapsed < maxDuration))
            {
                Thread.Sleep(retryInterval);
                success = task();
            }

            return success;
        }
    }
}
