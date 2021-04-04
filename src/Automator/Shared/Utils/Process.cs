namespace Automator.Shared.Utils
{
    public static class Process
    {
        public static void KillProcessByName(string processName)
        {
            foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(processName))
            {
                process.Kill();
            }
        }
    }
}
