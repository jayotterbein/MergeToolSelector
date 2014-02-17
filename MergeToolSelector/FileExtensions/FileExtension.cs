using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var ret = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c != '$') // start custom parsing with $
                {
                    ret.Append(c);
                    continue;
                }


                i++;
                // string ends with a $, place this in the result
                if (i == str.Length)
                {
                    ret.Append('$');
                    break;
                }

                c = str[i];
                if (c == '$') // double $s become single $s
                {
                    ret.Append('$');
                    continue;
                }

                // get the full number after $ to handle args >9.  don't bother with larger than 3 (>999 args)
                var subStr = str.Substring(i, Math.Min(3, str.Length - i));
                var numRegex = Regex.Match(subStr, @"^\d+", RegexOptions.None);
                if (!numRegex.Success)
                {
                    // this would be $ followed by a non-numeric, so do no replacement
                    ret.Append('$').Append(c);
                    continue;
                }

                // go beyond the full digits that were found
                i += (numRegex.Value.Length - 1);

                var num = int.Parse(numRegex.Value);
                // only replace if the arg exists, otherwise use nothing
                if (num < args.Count)
                {
                    ret.Append(args[num]);
                }
            }
            return ret.ToString();
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
