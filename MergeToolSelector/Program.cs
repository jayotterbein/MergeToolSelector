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
using MergeToolSelector.Utility;
using NLog;

namespace MergeToolSelector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var processExecuter = new ProcessExecuter();
            var messageDisplayer = new MessageDisplayer();
            var fileProvider = new FileProvider();
            var fileExtPersister = new FileExtensionPersister(fileProvider);
            var fileExtLocator = new FileExtensionLocator(fileExtPersister, new BuiltInFileExtensions());
            var clp = new CommandLineParser(processExecuter, messageDisplayer, fileExtLocator);
            clp.Parse(args);
        }
    }
}