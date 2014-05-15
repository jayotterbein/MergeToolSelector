using System.Collections.Generic;

namespace MergeToolSelector.Utility.FileExtensions
{
    public interface IFileExtensionLocator
    {
        FileExtension GetFileExtension(IList<string> paths);
    }
}