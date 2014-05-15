using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MergeToolSelector.Utility.FileExtensions;
using MergeToolSelector.Utility.Settings;
using Moq;
using NUnit.Framework;

namespace MergeToolSelectorTests.FileExtensionsTests
{
    [TestFixture]
    public class FileExtensionLocatorTests
    {
        private IFileExtensionPersister _savedFileExtensionPersisterWithDefault;
        private IFileExtensionPersister _savedFileExtensionPersisterWithoutDefault;
        private IFileExtensionPersister _builtInFileExtensionPersister;

        [SetUp]
        public void Setup()
        {
            // any executable that exists
            var exe = Assembly.GetExecutingAssembly().Location;

            var savedFileExtensionPersisterWithDefault = new Mock<IFileExtensionPersister>();
            savedFileExtensionPersisterWithDefault
                .Setup(x => x.LoadFileExtensions())
                .Returns(new[]
                         {
                             new FileExtension
                             {
                                 FileExts = new[] {"saved"},
                                 Command = exe,
                                 DiffArguments = "saved diff arguments",
                                 MergeArguments = "saved merge arguments",
                             },
                             new FileExtension
                             {
                                 FileExts = null,
                                 Command = exe,
                                 DiffArguments = "default diff arguments",
                                 MergeArguments = "default merge arguments",
                             }
                         });
            _savedFileExtensionPersisterWithDefault = savedFileExtensionPersisterWithDefault.Object;

            var savedFileExtensionPersisterWithoutDefault = new Mock<IFileExtensionPersister>();
            savedFileExtensionPersisterWithoutDefault
                .Setup(x => x.LoadFileExtensions())
                .Returns(new[]
                         {
                             new FileExtension
                             {
                                 FileExts = new[] {"saved"},
                                 Command = exe,
                                 DiffArguments = "saved diff arguments",
                                 MergeArguments = "saved merge arguments",
                             },
                         });
            _savedFileExtensionPersisterWithoutDefault = savedFileExtensionPersisterWithoutDefault.Object;

            var builtInFileExtensionPersister = new Mock<IFileExtensionPersister>();
            builtInFileExtensionPersister
                .Setup(x => x.LoadFileExtensions())
                .Returns(new[]
                         {
                             new FileExtension
                             {
                                 FileExts = new[] {"builtin"},
                                 Command = exe,
                                 DiffArguments = "builtin diff arguments",
                                 MergeArguments = "builtin merge arguments",
                             },
                             new FileExtension
                             {
                                 FileExts = null,
                                 Command = exe,
                                 DiffArguments = "builtin default diff arguments",
                                 MergeArguments = "builtin default merge arguments",
                             }
                         });
            _builtInFileExtensionPersister = builtInFileExtensionPersister.Object;
        }


        [Test]
        public void Locates_saved_matched_fileext_first()
        {
            var locator = new FileExtensionLocator(_savedFileExtensionPersisterWithDefault, _builtInFileExtensionPersister);
            var fileExt = locator.GetFileExtension(new[] {@"c:\something\file.saved"});
            Assert.That(fileExt, Has.Property("DiffArguments").EqualTo("saved diff arguments"));
        }

        [Test]
        public void Locates_saved_defaulted_fileext_before_explicit_builtin()
        {
            var locator = new FileExtensionLocator(_savedFileExtensionPersisterWithDefault, _builtInFileExtensionPersister);
            var fileExt = locator.GetFileExtension(new[] { @"c:\something\file.notsaved" });
            Assert.That(fileExt, Has.Property("DiffArguments").EqualTo("default diff arguments"));
        }

        [Test]
        public void Locates_builtin_matched_before_builtin_default()
        {
            var locator = new FileExtensionLocator(_savedFileExtensionPersisterWithoutDefault, _builtInFileExtensionPersister);
            var fileExt = locator.GetFileExtension(new[] { @"c:\something\file.builtin" });
            Assert.That(fileExt, Has.Property("DiffArguments").EqualTo("builtin diff arguments"));
        }

        [Test]
        public void Locates_builtin_default()
        {
            var locator = new FileExtensionLocator(_savedFileExtensionPersisterWithoutDefault, _builtInFileExtensionPersister);
            var fileExt = locator.GetFileExtension(new[] { @"c:\something\file.randomExt" });
            Assert.That(fileExt, Has.Property("DiffArguments").EqualTo("builtin default diff arguments"));
        }
    }
}