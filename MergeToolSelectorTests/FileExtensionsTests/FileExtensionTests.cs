using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeToolSelector.FileExtensions;
using NUnit.Framework;

namespace MergeToolSelectorTests.FileExtensionsTests
{
    [TestFixture]
    public class FileExtensionTests
    {
        [Test]
        public void IsForExtension_returns_false_with_no_ext()
        {
            var fileExtension = new FileExtension
                                {
                                    FileExts = null,
                                    Command = "some.exe",
                                    DiffArguments = "diff arguments",
                                    MergeArguments = "merge arguments",
                                };
            Assert.That(fileExtension.IsForExtension(new[] { "whatever.exe" }), Is.False);
        }

        [Test]
        public void IsForExtension_returns_false_with_no_paths()
        {
            var fileExtension = new FileExtension
            {
                FileExts = null,
                Command = "some.exe",
                DiffArguments = "diff arguments",
                MergeArguments = "merge arguments",
            };
            Assert.That(fileExtension.IsForExtension(null), Is.False, "fails with null paths argument");
            Assert.That(fileExtension.IsForExtension(Enumerable.Empty<string>()), Is.False, "fails with empty paths argument");
            Assert.That(fileExtension.IsForExtension(new[] { (string)null }), Is.False, "fails with a paths list that contains null");
        }

        [Test]
        public void IsForExtension_returns_true_when_matching()
        {
            var fileExtension = new FileExtension
            {
                FileExts = new [] { "cs"},
                Command = "some.exe",
                DiffArguments = "diff arguments",
                MergeArguments = "merge arguments",
            };
            Assert.That(fileExtension.IsForExtension(new[] {"someFile.cs"}), Is.True);
        }

        [Test]
        public void FileExts_prepends_periods()
        {
            var fileExtension = new FileExtension
            {
                FileExts = new[] { "cs" },
            };
            Assert.That(fileExtension.FileExts, Contains.Item(".cs").Using((IComparer) StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void GetEffectiveDiffArguments_replaces_paths_and_labels()
        {
            var fileExtension = new FileExtension
            {
                FileExts = null,
                Command = "some.exe",
                DiffArguments = "diff arguments: $4 $2 $1 $3 $1",
            };
            var effectiveDiff = fileExtension.GetEffectiveDiffArguments(new [] { "diff", @"c:\left\path\file.ext", @"\\server\path\file.ext", "left label", "right label" });
            Assert.That(effectiveDiff, Is.EqualTo(@"diff arguments: right label \\server\path\file.ext c:\left\path\file.ext left label c:\left\path\file.ext"));
        }

        [Test]
        public void GetEffectiveDiffArguments_ignores_double_dollarsigns()
        {
            var fileExtension = new FileExtension
            {
                FileExts = null,
                Command = "some.exe",
                DiffArguments = "diff arguments: $1 $$1",
            };
            var effectiveDiff = fileExtension.GetEffectiveDiffArguments(new[] { "diff", "first" });
            Assert.That(effectiveDiff, Is.EqualTo(@"diff arguments: first $1"));
        }

        [Test]
        public void GetEffectiveDiffArguments_doesnt_replace_args_in_args()
        {
            var fileExtension = new FileExtension
            {
                FileExts = null,
                Command = "some.exe",
                DiffArguments = "diff arguments: $1 $2",
            };
            var effectiveDiff = fileExtension.GetEffectiveDiffArguments(new[] { "diff", "first-$2", "second" });
            Assert.That(effectiveDiff, Is.EqualTo(@"diff arguments: first-$2 second"));
        }

        [Test]
        public void GetEffectiveDiffArguments_ignores_dollarsign_followed_by_non_numeric()
        {
            var fileExtension = new FileExtension
            {
                FileExts = null,
                Command = "some.exe",
                DiffArguments = "diff arguments: $a $1 $$c",
            };
            var effectiveDiff = fileExtension.GetEffectiveDiffArguments(new[] { "diff", "first", "second", "third" });
            Assert.That(effectiveDiff, Is.EqualTo(@"diff arguments: $a first $c"));
        }

        [Test]
        public void GetEffectiveDiffArguments_blanks_non_existent_args()
        {
            var fileExtension = new FileExtension
            {
                FileExts = null,
                Command = "some.exe",
                DiffArguments = "diff arguments: $1 $100",
            };
            var effectiveDiff = fileExtension.GetEffectiveDiffArguments(new[] { "diff", "first", "second", "third" });
            Assert.That(effectiveDiff, Is.EqualTo(@"diff arguments: first "));
        }
    }
}
