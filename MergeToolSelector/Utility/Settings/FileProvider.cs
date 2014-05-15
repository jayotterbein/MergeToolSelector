using System;
using System.IO;
using NLog;

namespace MergeToolSelector.Utility.Settings
{
    public class FileProvider : IFileProvider
    {
        private readonly Logger _logger;
        private readonly string _settingsFolderPath;
        
        public FileProvider()
        {
            _logger = LogManager.GetCurrentClassLogger();

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _logger.Trace("AppData: " + appData);
            _settingsFolderPath = Path.Combine(appData, "MergeToolSelector");
            _logger.Trace("Settings folder path: " + _settingsFolderPath);
        }

        public Stream GetFileExtensionsFile()
        {
            var file = Path.Combine(_settingsFolderPath, "FileExtensions.json");
            _logger.Trace("INI: " + file);

            return (!File.Exists(file))
                ? null
                : new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}