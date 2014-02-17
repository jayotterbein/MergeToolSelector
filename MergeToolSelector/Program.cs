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
                Exec((fileEx, argList) => fileEx.GetEffectiveDiffArguments(argList), args);
            }
            else if (string.Equals(args[0], "merge", StringComparison.OrdinalIgnoreCase))
            {
                Exec((fileEx, argList) => fileEx.GetEffectiveMergeArguments(argList), args);
            }
        }

        private static void Exec(Func<FileExtension, IList<string>, string> effectiveFunc, IList<string> args)
        {
            var fileEx = GetFileExtension(args);
            var arguments = effectiveFunc(fileEx, args);
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

        private static FileExtension GetFileExtension(IEnumerable<string> args)
        {
            var fileExtPersister = new FileExtensionPersister(new FileProvider());
            var fileExtensionLocator = new FileExtensionLocator(fileExtPersister, new BuiltInFileExtensions());

            var argsForPaths = args.Where(File.Exists).ToArray();
            return fileExtensionLocator.GetFileExtension(argsForPaths);
        }
    }
}