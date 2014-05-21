using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MergeToolSelector.Utility.FileExtensions;
using MergeToolSelector.Utility.Settings;
using Moq;
using NUnit.Framework;

namespace MergeToolSelectorTests.SettingsTests
{
    [TestFixture]
    public class FileExtensionPersisterTests : FileExtensionPersister
    {
        public FileExtensionPersisterTests() : base(null)
        {
        }

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
            Assert.That(obtainedFileExtensions[0], Has.Property("FileExts").EquivalentTo(new[] { ".cs", ".other" }).Using((IComparer)StringComparer.OrdinalIgnoreCase));
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

        [Test]
        public void Saved_extensions_can_be_reloaded()
        {
            var fileExt = new FileExtension
            {
                Id = Guid.Empty,
                Command = "command",
                DiffArguments = "diff",
                MergeArguments = "merge",
                FileExts = new[] {".ext"}
            };

            var file = Path.GetTempFileName();
            try
            {
                using (var stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    var fileProvider = new Mock<IFileProvider>();
                    fileProvider.Setup(x => x.GetFileExtensionsFile()).Returns(stream);
                    var fileExtPersister = new FileExtensionPersister(fileProvider.Object);
                    
                    fileExtPersister.SaveFileExtensions(new [] { fileExt });
                }
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fileProvider = new Mock<IFileProvider>();
                    fileProvider.Setup(x => x.GetFileExtensionsFile()).Returns(stream);
                    var fileExtPersister = new FileExtensionPersister(fileProvider.Object);

                    var loadedFileExt = fileExtPersister.LoadFileExtensions();
                    Assert.That(loadedFileExt[0], Is.EqualTo(fileExt).Using(new FileExtensionEqualityComparer()));
                }

            }
            finally
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void MergeFileExtTest()
        {
            var currentFileExt = new[]
            {
                new FileExtension
                {
                    Id = Guid.Parse("138bca89-84a3-42f0-a33b-28cc3754578e"),
                    Command = "current command - unchanged",
                    DiffArguments = "diff args - unchanged",
                    MergeArguments = "merge args - unchanged",
                    FileExts = new[] {".js"}
                },
                new FileExtension
                {
                    Id = Guid.Parse("966b5a93-03b6-4923-8026-3b7c3ca4a318"),
                    Command = "current command - to change",
                    DiffArguments = "current command - to change",
                    MergeArguments = "current command - to change",
                    FileExts = new[] {".cs"}
                }
            };
            var newFileExt = new[]
            {
                new FileExtension
                {
                    Id = Guid.Parse("198a0d31-8d45-42f6-9367-185f8fee325a"),
                    Command = "new command - not replacing",
                    DiffArguments = "new command - not replacing",
                    MergeArguments = "new command - not replacing",
                    FileExts = new[] {".txt"}
                },
                new FileExtension
                {
                    Id = Guid.Parse("966b5a93-03b6-4923-8026-3b7c3ca4a318"),
                    Command = "changed command",
                    DiffArguments = "changed command",
                    MergeArguments = "changed command",
                    FileExts = new[] {".cs", ".c", ".cpp"}
                }
            };

            var expectedFileExt = new[]
            {
                new FileExtension
                {
                    Id = Guid.Parse("138bca89-84a3-42f0-a33b-28cc3754578e"),
                    Command = "current command - unchanged",
                    DiffArguments = "diff args - unchanged",
                    MergeArguments = "merge args - unchanged",
                    FileExts = new[] {".js"}
                },
                new FileExtension
                {
                    Id = Guid.Parse("198a0d31-8d45-42f6-9367-185f8fee325a"),
                    Command = "new command - not replacing",
                    DiffArguments = "new command - not replacing",
                    MergeArguments = "new command - not replacing",
                    FileExts = new[] {".txt"}
                },
                new FileExtension
                {
                    Id = Guid.Parse("966b5a93-03b6-4923-8026-3b7c3ca4a318"),
                    Command = "changed command",
                    DiffArguments = "changed command",
                    MergeArguments = "changed command",
                    FileExts = new[] {".cs", ".c", ".cpp"}
                }
            };

            var mergedFileExt = MergeFileExtensions(currentFileExt, newFileExt);
            Assert.That(mergedFileExt.OrderBy(x => x.Id), Is.EquivalentTo(expectedFileExt.OrderBy(x => x.Id)).Using(new FileExtensionEqualityComparer()));
        }

        private static Stream GetResource([CallerMemberName]string resourceName = "")
        {
            var fullName = string.Format(@"{0}.{1}.json", typeof(FileExtensionPersisterTests).Namespace, resourceName);
            var resourceStream = typeof(FileExtensionPersisterTests).Assembly.GetManifestResourceStream(fullName);
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
