﻿using System;
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
                                               MergeArguments = @"""-d=$2"" ""-b=$1"" ""-s=$3"" ""-r=$4"" ""-dn=$6"" ""-sn=$5"" ""-bn=$7""",
                                           };
                // if beyond compare exists, use it as a default text diff/merge within semantic merge
                if (File.Exists(beyondCompare))
                {
                    var edt = string.Format(@" ""-edt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$3: #sourcesymbolic"""" """"/righttitle=$4: #destinationsymbolic""""""", beyondCompare);
                    var emt = string.Format(@" ""-emt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"#basefile"""" """"#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$5: #sourcesymbolic"""" """"/righttitle=$6: #destinationsymbolic"""" """"/centertitle=$7"""" """"/outputtitle=$8""""""", beyondCompare);
                    var e2mt = string.Format(@" ""-e2mt=""""{0}"""" """"#sourcefile"""" """"#destinationfile"""" """"/mergeoutput=#output"""" /nobackups /leftreadonly /rightreadonly /solo """"/lefttitle=$5: #sourcesymbolic"""" """"/righttitle=$6: #destinationsymbolic"""" """"/outputtitle=$8""""""", beyondCompare);
                    
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