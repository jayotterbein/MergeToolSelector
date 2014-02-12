using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MergeToolSelector.FileExtensions
{
    public class FileExtension
    {
        public string FileExt { get; set; }

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

        public bool IsForExtension(IEnumerable<string> paths)
        {
            return FileExt != null
                   && paths.Any(p => p != null && p.EndsWith(FileExt, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
