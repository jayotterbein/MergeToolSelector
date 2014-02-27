using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using MergeToolSelector.FileExtensions;
using MergeToolSelector.Settings;

namespace MergeToolSelector.Utility
{
    public class CommandLineParser
    {
        private readonly IProcessExecuter _processExecuter;
        private readonly IMessageDisplayer _messageDisplayer;
        private readonly IFileExtensionLocator _fileExtensionLocator;

        public CommandLineParser(IProcessExecuter processExecuter, IMessageDisplayer messageDisplayer, IFileExtensionLocator fileExtensionLocator)
        {
            _processExecuter = processExecuter;
            _messageDisplayer = messageDisplayer;
            _fileExtensionLocator = fileExtensionLocator;
        }

        public void Parse(IList<string> args)
        {
            var cmd = GetCommand(args);
            if (cmd.Equals("diff", StringComparison.Ordinal))
            {
                Exec((fileExt, a) => fileExt.GetEffectiveDiffArguments(a), args);
            }
            else if (cmd.Equals("merge", StringComparison.Ordinal))
            {
                Exec((fileExt, a) => fileExt.GetEffectiveMergeArguments(a), args);
            }
            else
            {
                _messageDisplayer.Display("Help: ");
            }
        }

        private static string GetCommand(IList<string> args)
        {
            if (args != null && args.Any())
            {
                var cmd = (args.First() ?? string.Empty).ToLower();
                return cmd;
            }
            return "help";
        }

        private void Exec(Func<FileExtension, IList<string>, string> getEffectiveArgsFunc, IList<string> args)
        {
            var fileEx = _fileExtensionLocator.GetFileExtension(args);
            var cmdlineArg = getEffectiveArgsFunc(fileEx, args);
            _processExecuter.Start(fileEx.Command, cmdlineArg);
        }
    }
}