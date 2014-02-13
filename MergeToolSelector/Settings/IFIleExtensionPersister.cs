using System.Collections.Generic;
using MergeToolSelector.FileExtensions;

namespace MergeToolSelector.Settings
{
    public interface IFileExtensionPersister
    {
        IList<FileExtension> LoadFileExtensions();
    }
}