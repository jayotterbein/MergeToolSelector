using System.Collections.Generic;
using System.IO;
using System.Linq;
using MergeToolSelector.Utility.Settings;

namespace MergeToolSelector.Utility.FileExtensions
{
    public class FileExtensionLocator : IFileExtensionLocator
    {
        private readonly IFileExtensionPersister _fileExtensionPersister;
        private readonly IFileExtensionPersister _builtInFileExtensionPersister;

        public FileExtensionLocator(IFileExtensionPersister fileExtensionPersister, IFileExtensionPersister builtInFileExtensionPersister)
        {
            _fileExtensionPersister = fileExtensionPersister;
            _builtInFileExtensionPersister = builtInFileExtensionPersister;
        }

        public FileExtension GetFileExtension(IList<string> paths)
        {
            var savedExtensions = _fileExtensionPersister
                .LoadFileExtensions()
                .ToArray();

            // match saved file extensions that exactly line up with one of the given extensions
            var matchingExtension = savedExtensions.FirstOrDefault(x => x.IsForExtension(paths));
            if (matchingExtension != null)
                return matchingExtension;

            // match saved file extension if it is a fallback
            matchingExtension = savedExtensions.FirstOrDefault(x => x.FileExts == null);
            if (matchingExtension != null)
                return matchingExtension;

            // match any built in extensions, using the same rules
            var builtInExtensions = _builtInFileExtensionPersister.LoadFileExtensions().ToArray();
            return builtInExtensions.FirstOrDefault(x => x.IsForExtension(paths)) ?? builtInExtensions.FirstOrDefault(x => x.FileExts == null);
        }
    }
}
