using System.IO;

namespace MergeToolSelector.Utility.Settings
{
    public interface IFileProvider
    {
        Stream GetFileExtensionsFile();
    }
}