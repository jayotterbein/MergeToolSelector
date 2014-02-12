using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeToolSelector.FileExtensions
{
    public static class BuiltInFileExtensions
    {
        public static readonly IList<FileExtension> FileExtensions;

        static BuiltInFileExtensions()
        {
            FileExtensions =
                new[]
                {
                    new FileExtension
                    {
                        Command = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Beyond Compare 3", "BComp.exe"),
                        DiffArguments =  @"""$leftPath"" ""$rightPath"" /nobackups /leftreadonly /solo ""/lefttitle=$leftLabel"" ""/righttitle=$rightLabel""",
                        MergeArguments = @"""$leftPath"" ""$rightPath"" ""$centerPath"" ""$destPath"" /nobackups /leftreadonly /rightreadonly /solo ""/lefttitle=$leftLabel"" ""/righttitle=$rightLabel"" ""/centertitle=Automated Merge Result"" /outputtitle=""$destLabel""",
                    }
                };
        }
    }
}
