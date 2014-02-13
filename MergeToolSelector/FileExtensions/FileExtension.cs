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

        public string GetEffectiveDiffArguments(string leftPath, string rightPath, string leftLabel, string rightLabel)
        {
            var ret = DiffArguments
                .Replace("$leftPath", leftPath)
                .Replace("$rightPath", rightPath)
                .Replace("$leftLabel", leftLabel)
                .Replace("$rightLabel", rightLabel);
            return ret;
        }

        public string GetEffectiveMergeArguments(string leftPath, string rightPath, string centerPath, string destPath, string leftLabel, string rightLabel, string centerLabel, string destLabel)
        {
            var ret = MergeArguments
                .Replace("$leftPath", leftPath)
                .Replace("$rightPath", rightPath)
                .Replace("$centerPath", centerPath)
                .Replace("$destPath", destPath)
                .Replace("$leftLabel", leftLabel)
                .Replace("$rightLabel", rightLabel)
                .Replace("$centerLabel", centerLabel)
                .Replace("$destLabel", destLabel);
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
