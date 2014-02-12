using System.IO;

namespace MergeToolSelector.Settings
{
    public interface IFileProvider
    {
        Stream GetFileExtensionsFile();
    }
}