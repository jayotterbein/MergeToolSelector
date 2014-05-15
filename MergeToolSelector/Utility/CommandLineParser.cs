using System;
using System.Collections.Generic;
using System.Linq;
using MergeToolSelector.Forms;
using MergeToolSelector.Utility.FileExtensions;

namespace MergeToolSelector.Utility
{
    public class CommandLineParser
    {
        private readonly IProcessExecuter _processExecuter;
        private readonly IFormDisplayer _formDisplayer;
        private readonly IFileExtensionLocator _fileExtensionLocator;

        public CommandLineParser(IProcessExecuter processExecuter, IFormDisplayer formDisplayer, IFileExtensionLocator fileExtensionLocator)
        {
            _processExecuter = processExecuter;
            _formDisplayer = formDisplayer;
            _fileExtensionLocator = fileExtensionLocator;
        }

        public void Parse(IList<string> args)
        {
            var cmd = GetCommand(args);
            if (cmd.Equals("diff", StringComparison.Ordinal))
            {
                Exec(fileExt => fileExt.GetEffectiveDiffArguments(args), args);
            }
            else if (cmd.Equals("merge", StringComparison.Ordinal))
            {
                Exec(fileExt => fileExt.GetEffectiveMergeArguments(args), args);
            }
            else
            {
                _formDisplayer.Display(new Main());
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

        private void Exec(Func<FileExtension, string> getEffectiveArgsFunc, IList<string> args)
        {
            var fileEx = _fileExtensionLocator.GetFileExtension(args);
            var cmdlineArg = getEffectiveArgsFunc(fileEx);
            _processExecuter.Start(fileEx.Command, cmdlineArg);
        }
    }
}