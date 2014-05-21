using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MergeToolSelector.Utility.FileExtensions;
using Newtonsoft.Json.Linq;
using NLog;

namespace MergeToolSelector.Utility.Settings
{
    public class FileExtensionPersister : IFileExtensionPersister
    {
        private const string FileExtKey = "FileExtensions";
        private const int BufferSize = 8192;
        private static readonly Encoding Encoding = Encoding.UTF8;

        private readonly IFileProvider _fileProvider;
        private readonly Logger _logger;

        public FileExtensionPersister(IFileProvider fileProvider)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _fileProvider = fileProvider;
        }

        public IList<FileExtension> LoadFileExtensions()
        {
            using (var stream = _fileProvider.GetFileExtensionsFile())
            {
                var jobject = GetJObject(stream);
                return GetFileExtensionsFrom(jobject);
            }
        }

        private JObject GetJObject(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding, false, BufferSize, leaveOpen: true))
            {
                var json = sr.ReadToEnd();
                if (string.IsNullOrWhiteSpace(json))
                    return new JObject();
                var jobject = JObject.Parse(json);
                return jobject;
            }
        }

        private IList<FileExtension> GetFileExtensionsFrom(JObject jobject)
        {
            JToken fileExtensions;
            if (!jobject.TryGetValue(FileExtKey, StringComparison.Ordinal, out fileExtensions))
                return new List<FileExtension>();

            var ret = new List<FileExtension>();
            foreach (var fileExtension in fileExtensions.Select(x => x.ToObject<FileExtension>()))
            {
                // backwards compatability before Ids were added
                if (fileExtension.Id == Guid.Empty)
                {
                    fileExtension.Id = Guid.NewGuid();
                }
                _logger.Trace("Load FileExt: " + fileExtension);
                ret.Add(fileExtension);
            }
            return ret;
        }

        public void SaveFileExtensions(params FileExtension[] fileExtensionses)
        {
            using (var stream = _fileProvider.GetFileExtensionsFile())
            {
                var jobject = GetJObject(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var currentFileExtensions = GetFileExtensionsFrom(jobject);
                var mergedFileExtensions = MergeFileExtensions(currentFileExtensions, fileExtensionses);
                jobject[FileExtKey] = JToken.FromObject(mergedFileExtensions);
                using (var sw = new StreamWriter(stream, Encoding, BufferSize, leaveOpen:true))
                {
                    sw.Write(jobject);
                }
            }
        }

        protected static IEnumerable<FileExtension> MergeFileExtensions(IList<FileExtension> currentFileExtensions, IEnumerable<FileExtension> newFileExtensions)
        {
            var ignoredIndecies = new List<int>();
            foreach (var newFileExt in newFileExtensions)
            {
                for (var i = 0; i < currentFileExtensions.Count; i++)
                {
                    var currentFileExt = currentFileExtensions[i];
                    if (currentFileExt.Id == newFileExt.Id)
                    {
                        ignoredIndecies.Add(i);
                        break;
                    }
                }
                if (newFileExt.Id == Guid.Empty)
                {
                    newFileExt.Id = Guid.NewGuid();
                }
                yield return newFileExt;
            }
            foreach (var i in Enumerable.Range(0, currentFileExtensions.Count).Where(i => !ignoredIndecies.Contains(i)))
            {
                yield return currentFileExtensions[i];
            }
        }
    }
}