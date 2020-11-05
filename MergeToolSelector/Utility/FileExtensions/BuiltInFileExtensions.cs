using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MergeToolSelector.Utility.Settings;
using NLog;

namespace MergeToolSelector.Utility.FileExtensions
{
    public class BuiltInFileExtensions : IFileExtensionPersister
    {
        private readonly Logger _logger;

        public BuiltInFileExtensions()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        private void Trace(string message, [CallerMemberName] string caller = null, [CallerLineNumber] int lineNo = 0)
        {
            _logger.Trace($"{caller}:{lineNo} - {message}");
        }

        private IEnumerable<string> GetProgramFilesSearchPaths()
        {
            yield return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            yield return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            yield return Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Program Files");
        }

        private IEnumerable<FileExtension> GetDefaultFileExtensions()
        {
            Trace("Ignoring diffs for .feature.cs files; they are automatically generated");
            // for feature.cs files, ignore the result
            yield return new FileExtension
            {
                FileExts = new[] {"feature.cs"},
                Command = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
                DiffArguments = "/c exit 0",
                MergeArguments = "/c exit 0",
            };

            // if beyond compare exists, use it as a default fallback
            var beyondCompares = new[]
            {
                "Beyond Compare 4",
                "Beyond Compare 3",
            };
            Trace("Checking for beyond compare");
            string beyondCompare = null;
            foreach (var searchPath in GetProgramFilesSearchPaths())
            {
                foreach (var bcPath in beyondCompares)
                {
                    var bc = Path.Combine(searchPath, bcPath, "BComp.exe");
                    Trace($"Checking for beyond compare at {bc}");
                    if (File.Exists(bc))
                    {
                        Trace($"Found at {bc}");
                        beyondCompare = bc;
                        break;
                    }
                }
            }

            if (beyondCompare != null)
            {
                Trace($"Found beyond compare: {beyondCompare}");
                yield return new FileExtension
                             {
                                 Command = beyondCompare,
                                 DiffArguments = @"""$1"" ""$2"" /nobackups /leftreadonly /solo ""/lefttitle=$3"" ""/righttitle=$4""",
                                 MergeArguments = @"""$1"" ""$2"" ""$3"" ""$4"" /nobackups /leftreadonly /rightreadonly /solo ""/lefttitle=$5"" ""/righttitle=$6"" ""/centertitle=$7"" /outputtitle=""$8""",
                             };
            }

            // if semantic merge exists, use it for as a default for the known languages
            var semanticMergeSearchPaths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PlasticSCM4",
                    "semanticmerge", "semanticmergetool.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "semanticmerge", "semanticmergetool.exe"),
            };
            Trace("Checking for semantic merge");
            var semanticMerge = semanticMergeSearchPaths.FirstOrDefault(File.Exists);
            if (File.Exists(semanticMerge))
            {
                Trace($"Found semantic merge: {semanticMerge}");
                var semanticMergeFileExt = new FileExtension
                                           {
                                               FileExts = new [] { "cs", "vb", "java", "dpj", "c", "h" },
                                               Command = semanticMerge,
                                               DiffArguments = @"""-sn=$3"" ""-dn=$4"" ""-s=$1"" ""-d=$2""",
                                               MergeArguments = @"""-s=$1"" ""-d=$2"" ""-b=$3"" ""-r=$4"" ""-sn=$5"" ""-dn=$6"" ""-bn=$7""",
                                           };
                // if beyond compare exists, use it as a default text diff/merge within semantic merge
                if (File.Exists(beyondCompare))
                {
                    Trace("Adding beyond compare to semantic merge");
                    var edt = string.Format(@" ""-edt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$3: #sourcesymbolic"""" """"/righttitle=$4: #destinationsymbolic""""""", beyondCompare);
                    var emt = string.Format(@" ""-emt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"#basefile"""" """"#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$5: #sourcesymbolic"""" """"/righttitle=$6: #destinationsymbolic"""" """"/centertitle=$7"""" """"/outputtitle=$8""""""", beyondCompare);
                    var e2mt = string.Format(@" ""-e2mt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"/mergeoutput=#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$5: #sourcesymbolic"""" """"/righttitle=$6: #destinationsymbolic"""" """"/outputtitle=$8""""""", beyondCompare);
                    
                    semanticMergeFileExt.DiffArguments = semanticMergeFileExt.DiffArguments + edt;
                    semanticMergeFileExt.MergeArguments = semanticMergeFileExt.MergeArguments + emt + e2mt + edt.Replace("$3", "$5").Replace("$4", "$6");
                }
                yield return semanticMergeFileExt;
            }
        }

        public IList<FileExtension> LoadFileExtensions()
        {
            var result = GetDefaultFileExtensions().ToList();
            Trace($"Found {result.Count} default extension options");
            return result;
        }

        public void SaveFileExtensions(params FileExtension[] fileExtensions)
        {
            throw new NotImplementedException("Cannot save file extensions to the built-in provider");
        }
    }
}
