using System.Collections.Generic;

namespace MergeToolSelector.FileExtensions
{
    public interface IFileExtensionLocator
    {
        FileExtension GetFileExtension(IList<string> paths);
    }
}