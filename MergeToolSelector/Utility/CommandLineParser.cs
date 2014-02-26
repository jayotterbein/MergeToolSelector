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
            if (
                args == null
                || !args.Any()
                || args[0].EndsWith("help", StringComparison.OrdinalIgnoreCase)
                || args[0].EndsWith("?", StringComparison.OrdinalIgnoreCase))
            {
                _messageDisplayer.Display("The following are the allowed commands:");
            }
            else if (string.Equals(args[0], "diff", StringComparison.OrdinalIgnoreCase))
            {
                Exec((fileExt, a) => fileExt.GetEffectiveDiffArguments(a), args);
            }
            else if (string.Equals(args[0], "merge", StringComparison.OrdinalIgnoreCase))
            {
                Exec((fileExt, a) => fileExt.GetEffectiveMergeArguments(a), args);
            }
        }

        private void Exec(Func<FileExtension, IList<string>, string> getEffectiveArgsFunc, IList<string> args)
        {
            var fileEx = _fileExtensionLocator.GetFileExtension(args);
            var cmdlineArg = getEffectiveArgsFunc(fileEx, args);
            _processExecuter.Start(fileEx.Command, cmdlineArg);
        }
    }
}