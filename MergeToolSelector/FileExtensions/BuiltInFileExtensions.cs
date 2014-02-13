using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeToolSelector.Settings;
using Newtonsoft.Json;

namespace MergeToolSelector.FileExtensions
{
    public class BuiltInFileExtensions : IFileExtensionPersister
    {
        private static IEnumerable<FileExtension> GetDefaultFileExtensions()
        {
            // if beyond compare exists, use it as a default fallback
            var beyondCompare = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Beyond Compare 3", "BComp.exe");
            if (File.Exists(beyondCompare))
            {
                yield return new FileExtension
                             {
                                 Command = beyondCompare,
                                 DiffArguments = @"""$leftPath"" ""$rightPath"" /nobackups /leftreadonly /solo ""/lefttitle=$leftLabel"" ""/righttitle=$rightLabel""",
                                 MergeArguments = @"""$leftPath"" ""$rightPath"" ""$centerPath"" ""$destPath"" /nobackups /leftreadonly /rightreadonly /solo ""/lefttitle=$leftLabel"" ""/righttitle=$rightLabel"" ""/centertitle=$centerLabel"" /outputtitle=""$destLabel""",
                             };
            }

            // if semantic merge exists, use it for as a default for the known languages
            var semanticMerge = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PlasticSCM4", "semanticmerge", "semanticmergetool.exe");
            if (File.Exists(semanticMerge))
            {
                var semanticMergeFileExt = new FileExtension
                                           {
                                               FileExts = new [] { "cs", "vb", "java", "dpj", "c", "h" },
                                               Command = semanticMerge,
                                               DiffArguments = @"""-sn=$leftLabel"" ""-dn=$rightLabel"" ""-s=$leftPath"" ""-d=$rightPath""",
                                               MergeArguments = @"""-d=$rightPath"" ""-b=$leftPath"" ""-s=$centerPath"" ""-r=$destPath"" ""-dn=$rightLabel"" ""-sn=$leftLabel"" ""-bn=$destLabel""",
                                           };
                // if beyond compare exists, use it as a default text diff/merge within semantic merge
                if (File.Exists(beyondCompare))
                {
                    var edt = string.Format(@" ""-edt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$leftLabel: #sourcesymbolic"""" """"/righttitle=$rightLabel: #destinationsymbolic""""""", beyondCompare);
                    var emt = string.Format(@" ""-emt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"#basefile"""" """"#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$leftLabel: #sourcesymbolic"""" """"/righttitle=$rightLabel: #destinationsymbolic"""" """"/centertitle=$centerLabel"""" """"/outputtitle=$destLabel""""""", beyondCompare);
                    var e2mt = string.Format(@" ""-e2mt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"/mergeoutput=#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$leftLabel: #sourcesymbolic"""" """"/righttitle=$rightLabel: #destinationsymbolic"""" """"/outputtitle=$destLabel""""""", beyondCompare);
                    
                    semanticMergeFileExt.DiffArguments = semanticMergeFileExt.DiffArguments + edt;
                    semanticMergeFileExt.MergeArguments = semanticMergeFileExt.MergeArguments + emt + e2mt;
                }
                yield return semanticMergeFileExt;
            }
        }

        public IList<FileExtension> LoadFileExtensions()
        {
            return GetDefaultFileExtensions().ToArray();
        }
    }
}
