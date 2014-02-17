using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MergeToolSelector.FileExtensions
{
    public class FileExtension
    {
        private IList<string> _fileExts;

        public IList<string> FileExts
        {
            get { return _fileExts; }
            set { _fileExts = PrependPeriods(value); }
        }

        public string Command { get; set; }

        public string DiffArguments { get; set; }

        public string MergeArguments { get; set; }

        public string GetEffectiveDiffArguments(IList<string> args)
        {
            return ReplaceArgs(DiffArguments, args);
        }

        public string GetEffectiveMergeArguments(IList<string> args)
        {
            return ReplaceArgs(MergeArguments, args);
        }

        private static string ReplaceArgs(string str, IList<string> args)
        {
            var ret = str;
            for (var i = 1; i < args.Count; i++)
            {
                ret = ret.Replace("$" + i, args[i]);
            }
            return ret;
        }

        public bool IsForExtension(IEnumerable<string> paths)
        {
            return FileExts != null
                   && paths != null
                   && paths
                       .Where(p => p != null)
                       .Any(p => FileExts.Any(ext => p.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
        }

        private static IList<string> PrependPeriods(IList<string> fileExts)
        {
            if (fileExts == null || fileExts.Any() == false)
                return null;

            return fileExts
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(fileExt => (fileExt[0] == '.') ? fileExt : "." + fileExt)
                .ToArray();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
