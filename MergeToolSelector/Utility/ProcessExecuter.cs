using System;
using System.Diagnostics;

namespace MergeToolSelector.Utility
{
    public class ProcessExecuter : IProcessExecuter
    {
        public void Start(string command, string arguments)
        {
            var processStartInfo = new ProcessStartInfo(command)
                                   {
                                       Arguments = arguments,
                                       CreateNoWindow = true,
                                       WindowStyle = ProcessWindowStyle.Hidden,
                                       UseShellExecute = true,
                                       WorkingDirectory = Environment.CurrentDirectory,
                                   };

            using (var p = Process.Start(processStartInfo))
            {
                p.WaitForExit();
                Environment.Exit(p.ExitCode);
            }
        }
    }
}