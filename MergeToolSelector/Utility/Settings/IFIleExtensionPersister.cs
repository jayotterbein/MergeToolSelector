using System.Collections.Generic;
using MergeToolSelector.Utility.FileExtensions;

namespace MergeToolSelector.Utility.Settings
{
    public interface IFileExtensionPersister
    {
        IList<FileExtension> LoadFileExtensions();
    }
}