using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MergeToolSelector.FileExtensions;
using MergeToolSelector.Settings;
using NLog;

namespace MergeToolSelector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (
                args == null
                || !args.Any()
                || args[0].EndsWith("help", StringComparison.OrdinalIgnoreCase)
                || args[0].EndsWith("?", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The following are the allowed commands:");
            }
            else if (string.Equals(args[0], "diff", StringComparison.OrdinalIgnoreCase))
            {
                ExecuteDiff(args);
            }
            else if (string.Equals(args[0], "merge", StringComparison.OrdinalIgnoreCase))
            {
                ExecuteMerge(args);
            }
        }

        private static void ExecuteDiff(IList<string> args)
        {
            if (args.Count() != 5)
            {
                MessageBox.Show("Unable to diff without the following arguments (in order):" + Environment.NewLine + "leftPath rightPath leftLabel rightLabel");
                return;
            }
            var fileEx = GetFileExtension(args[1], args[2]);
            var arguments = fileEx.GetEffectiveDiffArguments(args[1], args[2], args[3], args[4]);
            StartProcess(fileEx.Command, arguments);
        }

        private static void ExecuteMerge(IList<string> args)
        {
            if (args.Count() != 9)
            {
                MessageBox.Show("Unable to merge without the following arguments (in order):" + Environment.NewLine + "leftPath rightPath centerPath destPath leftLabel rightLabel centerLabel destLabel");
                return;
            }
            var fileEx = GetFileExtension(args[1], args[2], args[3], args[4]);
            var arguments = fileEx.GetEffectiveMergeArguments(args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
            StartProcess(fileEx.Command, arguments);
        }

        private static void StartProcess(string command, string arguments)
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

        private static FileExtension GetFileExtension(params string[] paths)
        {
            var fileExtPersister = new FileExtensionPersister(new FileProvider());
            var fileExtensionLocator = new FileExtensionLocator(fileExtPersister, new BuiltInFileExtensions());
            return fileExtensionLocator.GetFileExtension(paths);
        }
    }
}