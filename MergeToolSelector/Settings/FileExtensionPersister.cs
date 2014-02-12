using System.Collections.Generic;
using System.IO;
using System.Linq;
using MergeToolSelector.FileExtensions;
using Newtonsoft.Json.Linq;
using NLog;

namespace MergeToolSelector.Settings
{
    public class FileExtensionPersister
    {
        private readonly IFileProvider _fileProvider;
        private readonly Logger _logger;

        public FileExtensionPersister(IFileProvider fileProvider)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _fileProvider = fileProvider;
        }

        public IList<FileExtension> ReadFileExtensions()
        {
            JToken fileExtensions;
            using (var stream = _fileProvider.GetFileExtensionsFile())
            {
                if (stream == null)
                    return new FileExtension[0];

                using (var sr = new StreamReader(stream))
                {
                    var json = sr.ReadToEnd();
                    var jobject = JObject.Parse(json);
                    fileExtensions = jobject["FileExtensions"];
                }
            }

            var ret = new List<FileExtension>();
            foreach (var fileExtension in fileExtensions.Select(x => x.ToObject<FileExtension>()))
            {
                _logger.Trace("FileExt: " + fileExtension);
                ret.Add(fileExtension);
            }
            return ret;
        }
    }
}