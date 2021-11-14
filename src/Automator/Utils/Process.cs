namespace Automator.Utils
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

        public static string ExecuteCmd(string command, string workingDirectory, bool waitToFinish = true)
        {
            var output = string.Empty;
            var error = string.Empty;

            using (var compiler = new System.Diagnostics.Process())
            {
                compiler.StartInfo.FileName = "cmd.exe";
                compiler.StartInfo.Arguments = $@"/c {command}";
                compiler.StartInfo.WorkingDirectory = workingDirectory;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.RedirectStandardError = true;
                compiler.Start();

                if (waitToFinish)
                {
                    output = compiler.StandardOutput.ReadToEnd();
                    error = compiler.StandardError.ReadToEnd();
                    compiler.WaitForExit();
                }
            }

            return error == string.Empty ? output : output + Environment.NewLine + error;
        }
    }
}
