using System;
using MergeToolSelector.Utility;
using MergeToolSelector.Utility.FileExtensions;
using MergeToolSelector.Utility.Settings;
using NLog;

namespace MergeToolSelector
{
    static public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Trace("Main():");
            for (var i = 0; i < args.Length; i++)
                logger.Trace(string.Format("  {0}: {1}", i, args[i]));

            var processExecuter = new ProcessExecuter();
            var messageDisplayer = new FormDisplayer();
            var fileProvider = new FileProvider();
            var fileExtPersister = new FileExtensionPersister(fileProvider);
            var fileExtLocator = new FileExtensionLocator(fileExtPersister, new BuiltInFileExtensions());
            var clp = new CommandLineParser(processExecuter, messageDisplayer, fileExtLocator);
            clp.Parse(args);
        }
    }
}