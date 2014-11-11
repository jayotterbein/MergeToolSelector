using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MergeToolSelector.Utility.Settings;

namespace MergeToolSelector.Utility.FileExtensions
{
    public class BuiltInFileExtensions : IFileExtensionPersister
    {
        private static IEnumerable<FileExtension> GetDefaultFileExtensions()
        {
            // if beyond compare exists, use it as a default fallback
            var beyondCompare3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Beyond Compare 3", "BComp.exe");
            var beyondCompare4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Beyond Compare 4", "BComp.exe");
            var beyondCompare = (File.Exists(beyondCompare4)) ? beyondCompare4 : beyondCompare3;
            if (File.Exists(beyondCompare))
            {
                yield return new FileExtension
                             {
                                 Command = beyondCompare,
                                 DiffArguments = @"""$1"" ""$2"" /nobackups /leftreadonly /solo ""/lefttitle=$3"" ""/righttitle=$4""",
                                 MergeArguments = @"""$1"" ""$2"" ""$3"" ""$4"" /nobackups /leftreadonly /rightreadonly /solo ""/lefttitle=$5"" ""/righttitle=$6"" ""/centertitle=$7"" /outputtitle=""$8""",
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
                                               DiffArguments = @"""-sn=$3"" ""-dn=$4"" ""-s=$1"" ""-d=$2""",
                                               MergeArguments = @"""-s=$1"" ""-d=$2"" ""-b=$3"" ""-r=$4"" ""-sn=$5"" ""-dn=$6"" ""-bn=$7""",
                                           };
                // if beyond compare exists, use it as a default text diff/merge within semantic merge
                if (File.Exists(beyondCompare))
                {
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
            return GetDefaultFileExtensions().ToArray();
        }

        public void SaveFileExtensions(params FileExtension[] fileExtensions)
        {
            throw new NotImplementedException("Cannot save file extensions to the built-in provider");
        }
    }
}
