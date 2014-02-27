using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MergeToolSelector.FileExtensions;
using MergeToolSelector.Settings;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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
                        FileExts = new [] { "cs" },
                        Command = @"c:\Program Files (x86)\Merge thingy\merge.exe",
                        DiffArguments = @"command line",
                        MergeArguments = @"merge command line",
                    },
                };
            
            var fileProvider = new Mock<IFileProvider>();
            fileProvider.Setup(x => x.GetFileExtensionsFile()).Returns(GetResource());

            var fileExtensionPersister = new FileExtensionPersister(fileProvider.Object);
            var obtainedFileExtensions = fileExtensionPersister.LoadFileExtensions();
            Assert.That(obtainedFileExtensions, Is.EquivalentTo(expectedFileExtensions).Using(new FileExtensionEqualityComparer()));
        }

        [Test]
        public void Load_prepends_period_to_fileext_without_one()
        {
            var fileProvider = new Mock<IFileProvider>();
            fileProvider.Setup(x => x.GetFileExtensionsFile()).Returns(GetResource());

            var fileExtensionPersister = new FileExtensionPersister(fileProvider.Object);
            var obtainedFileExtensions = fileExtensionPersister.LoadFileExtensions();
            Assert.That(obtainedFileExtensions, Has.Count.EqualTo(1));
            Assert.That(obtainedFileExtensions[0], Has.Property("FileExts").EquivalentTo(new[] {".cs", ".other"}).Using((IComparer) StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void Load_does_not_prepend_period_to_fileext_with_one()
        {
            var fileProvider = new Mock<IFileProvider>();
            fileProvider.Setup(x => x.GetFileExtensionsFile()).Returns(GetResource());

            var fileExtensionPersister = new FileExtensionPersister(fileProvider.Object);
            var obtainedFileExtensions = fileExtensionPersister.LoadFileExtensions();
            Assert.That(obtainedFileExtensions, Has.Count.EqualTo(1));
            Assert.That(obtainedFileExtensions[0], Has.Property("FileExts").EquivalentTo(new[] { ".cs", ".hasdot" }).Using((IComparer)StringComparer.OrdinalIgnoreCase));
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
                return StringListEquals(x.FileExts, y.FileExts)
                       && string.Equals(x.Command, y.Command, StringComparison.Ordinal)
                       && string.Equals(x.DiffArguments, y.DiffArguments, StringComparison.Ordinal)
                       && string.Equals(x.MergeArguments, y.MergeArguments, StringComparison.Ordinal);
            }

            static private bool StringListEquals(IEnumerable<string> list1, IEnumerable<string> list2)
            {
                if (list1 == null)
                    return list2 == null;
                if (list2 == null)
                    return false;

                return list1.SequenceEqual(list2, StringComparer.Ordinal);
            }

            public int GetHashCode(FileExtension obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
