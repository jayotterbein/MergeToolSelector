using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeToolSelector
{
    internal class Program
    {
        private static string BeyondCompare
        {
            get
            {
                var progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                return Path.Combine(progFiles, "Beyond Compare 3", "BComp.exe");
            }
        }

        private static string SemanticMerge
        {
            get
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(localAppData, "PlasticSCM4", "semanticmerge", "semanticmergetool.exe");
            }
        }

        private static readonly IDictionary<string, string> SemanticMergeExtensions =
            new Dictionary<string, string>
                {
                    {".cs", "csharp"},
                    {".vb", "vb"},
                    {".java", "java"},
                    {".dpj", "java"}
                };

        private static void Main(string[] args)
        {
            DebugArgs(args);
            var argList = ((args.Count() == 4)
                               ? GetDiff(args)
                               : GetMerge(args)
                          )
                .ToArray();
            DebugArgs(argList);

            var argsAsString = new StringBuilder();
            foreach (var a in argList.Skip(1))
            {
                argsAsString.Append(" ");
                if (a.Contains(" "))
                    argsAsString.Append("\"").Append(a).Append("\"");
                else
                    argsAsString.Append(a);
            }

            var processStartInfo = new ProcessStartInfo(argList[0])
                                       {
                                           Arguments = argsAsString.ToString(1, argsAsString.Length - 1),
                                           CreateNoWindow = true,
                                           WindowStyle = ProcessWindowStyle.Hidden,
                                           UseShellExecute = true,
                                           WorkingDirectory = Path.GetTempPath(),
                                       };

            using (var p = Process.Start(processStartInfo))
            {
                p.WaitForExit();
                Environment.Exit(p.ExitCode);
            }
        }

        private static void DebugArgs(IEnumerable<string> args)
        {
            var msg = args.Aggregate(new StringBuilder(), (builder, s) => builder.Append(s).AppendLine());
            //MessageBox.Show(msg.ToString());
        }


        private static IEnumerable<string> GetDiff(IList<string> args)
        {
            var leftLabel = args[0];
            var rightLabel = args[1];
            var leftPath = args[2];
            var rightPath = args[3];

            var lang = GetSemanticMergeLang(leftPath, rightPath);
            if (lang != null)
            {
                yield return SemanticMerge;
                yield return "-sn=" + leftLabel;
                yield return "-dn=" + rightLabel;
                yield return "-s=" + leftPath;
                yield return "-d=" + rightPath;
                yield return "-l=" + lang;
                yield return string.Format(
                    @"-edt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle={1}: #sourcesymbolic"""" """"/righttitle={2}: #destinationsymbolic""""",
                    BeyondCompare
                    ,leftLabel
                    ,rightLabel);
            }
            else
            {
                yield return BeyondCompare;
                yield return leftPath;
                yield return rightPath;
                yield return "/nobackups";
                yield return "/leftreadonly";
                yield return "/solo";
                yield return "/lefttitle=" + leftLabel;
                yield return "/righttitle=" + rightLabel;
            }
        }

        private static IEnumerable<string> GetMerge(IList<string> args)
        {
            var workingPath = args[0];
            var baselinePath = args[1];
            var otherPath = args[2];
            var destPath = args[3];
            var workingLabel = args[4];
            var otherLabel = args[5];
            var destLabel = args[6];

            var lang = GetSemanticMergeLang(workingPath, baselinePath, otherPath, destPath);
            if (lang != null)
            {
                yield return SemanticMerge;
                yield return "-d=" + workingPath;
                yield return "-b=" + baselinePath;
                yield return "-s=" + otherPath;
                yield return "-r=" + destPath;
                yield return "-dn=" + workingLabel;
                yield return "-sn=" + otherLabel;
                yield return "-bn=" + destLabel;
                yield return "-l=" + lang;
                yield return string.Format(
                    @"-e2mt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"/mergeoutput=#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle={1}: #sourcesymbolic"""" """"/righttitle={2}: #destinationsymbolic"""" """"/outputtitle={3}""""",
                    BeyondCompare,
                    otherLabel,
                    workingLabel,
                    destLabel);
                yield return string.Format(
                    @"-emt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"#basefile"""" """"#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle={1}: #sourcesymbolic"""" """"/righttitle={2}: #destinationsymbolic"""" """"/centertitle=(Automated Merge Result): #destinationsymbolic"""" """"/outputtitle={3}""""",
                    BeyondCompare,
                    otherLabel,
                    workingLabel,
                    destLabel);
            }
            else
            {
                yield return BeyondCompare;
                yield return otherPath;
                yield return workingPath;
                yield return baselinePath;
                yield return destPath;
                yield return "/lefttitle=" + otherLabel;
                yield return "/righttitle=" + workingLabel;
                yield return "/centertitle=Automated Merge Result";
                yield return "/outputtitle=" + destLabel;
                yield return "/leftreadonly";
                yield return "/rightreadonly";
                yield return "/solo";
                yield return "/nobackups";
            }
        }

        private static string GetSemanticMergeLang(params string[] paths)
        {
            foreach (var kv in SemanticMergeExtensions)
            {
                var ext = kv.Key;
                var lang = kv.Value;
                foreach (var p in paths)
                {
                    if (p.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                        return lang;
                }
            }
            return null;
        }
    }
}
