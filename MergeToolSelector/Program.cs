using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MergeToolSelector.Utility;
using MergeToolSelector.Utility.FileExtensions;
using MergeToolSelector.Utility.Settings;
using NLog;

namespace MergeToolSelector
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
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