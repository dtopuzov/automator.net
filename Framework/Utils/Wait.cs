using System;
using System.Diagnostics;
using System.Threading;

namespace Framework.Utils
{
    public static class Wait
    {
        public static bool Until(Func<bool> task, int timeout, int retryInterval = 500)
        {
            bool success = false;
            TimeSpan maxDuration = TimeSpan.FromSeconds(timeout);
            Stopwatch sw = Stopwatch.StartNew();
            while ((!success) && (sw.Elapsed < maxDuration))
            {
                Thread.Sleep(retryInterval);
                success = task();
            }

            return success;
        }
    }
}
