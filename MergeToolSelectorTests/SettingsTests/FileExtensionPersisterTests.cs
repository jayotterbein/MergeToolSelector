using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using MergeToolSelector.FileExtensions;
using MergeToolSelector.Settings;
using Moq;
using NUnit.Framework;

namespace MergeToolSelectorTests.SettingsTests
{
    [TestFixture]
    public class FileExtensionPersisterTests
    {

        [Test]
        public void Parses_diff_and_merge_lines()
        {
            var expectedFileExtensions =
                new[]
                {
                    new FileExtension
                    {
                        FileExt = "cs",
                        Command = @"c:\Program Files (x86)\Merge thingy\merge.exe",
                        DiffArguments = @"command line",
                        MergeArguments = @"merge command line",
                    },
                };
            
            var fileProvider = new Mock<IFileProvider>();
            fileProvider.Setup(x => x.GetFileExtensionsFile()).Returns(GetResource());

            var fileExtensionPersister = new FileExtensionPersister(fileProvider.Object);
            var obtainedFileExtensions = fileExtensionPersister.ReadFileExtensions();
            Assert.That(obtainedFileExtensions, Is.EquivalentTo(expectedFileExtensions).Using(new FileExtensionEqualityComparer()));
        }

        private static Stream GetResource([CallerMemberName]string resourceName = "")
        {
            var fullName = string.Format(@"{0}.{1}.json", typeof (FileExtensionPersisterTests).Namespace, resourceName);
            var resourceStream = typeof (FileExtensionPersisterTests).Assembly.GetManifestResourceStream(fullName);
            return resourceStream;
        }

        private sealed class FileExtensionEqualityComparer : IEqualityComparer<FileExtension>
        {
            public bool Equals(FileExtension x, FileExtension y)
            {
                return string.Equals(x.FileExt, y.FileExt, StringComparison.Ordinal)
                       && string.Equals(x.Command, y.Command, StringComparison.Ordinal)
                       && string.Equals(x.DiffArguments, y.DiffArguments, StringComparison.Ordinal)
                       && string.Equals(x.MergeArguments, y.MergeArguments, StringComparison.Ordinal);
            }

            public int GetHashCode(FileExtension obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
