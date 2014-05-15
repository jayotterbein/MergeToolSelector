using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MergeToolSelector.Utility;
using MergeToolSelector.Utility.FileExtensions;
using Moq;
using NUnit.Framework;

namespace MergeToolSelectorTests.UtilityTests
{
    [TestFixture]
    public class CommandLineParserTests
    {
        [Test]
        public void No_args_displays_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var formDisplayer = new Mock<IFormDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, formDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new string[] {});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            formDisplayer.Verify(x => x.Display(It.IsAny<Form>()), Times.Exactly(1));
        }

        [Test]
        public void Help_arg_displays_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var formDisplayer = new Mock<IFormDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, formDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new[] {"hElp"});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            formDisplayer.Verify(x => x.Display(It.IsAny<Form>()), Times.Exactly(1));
        }

        [Test]
        public void Question_arg_displays_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var formDisplayer = new Mock<IFormDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, formDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new[] {"?"});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            formDisplayer.Verify(x => x.Display(It.IsAny<Form>()), Times.Exactly(1));
        }


        [Test]
        public void Unrecognized_command_gives_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var formDisplayer = new Mock<IFormDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, formDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new[] {"nothing"});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            formDisplayer.Verify(x => x.Display(It.IsAny<Form>()), Times.Once());
        }

        [Test]
        public void Diff()
        {
            var args = new[] {"DIff", "path", "path2", "left label", "right label"};

            var fileExt = new Mock<FileExtension>();
            fileExt.Setup(x => x.IsForExtension(It.IsAny<IEnumerable<string>>())).Returns(true);
            fileExt.Setup(x => x.GetEffectiveDiffArguments(It.IsAny<IList<string>>())).Returns("effective arguments");
            fileExt.Setup(x => x.Command).Returns("command");

            var fileExtLocator = new Mock<IFileExtensionLocator>();
            fileExtLocator.Setup(x => x.GetFileExtension(It.IsAny<IList<string>>())).Returns(fileExt.Object);

            var processExecMock = new Mock<IProcessExecuter>();
            var parser = new CommandLineParser(processExecMock.Object, Mock.Of<IFormDisplayer>(), fileExtLocator.Object);
            parser.Parse(args);


            processExecMock.Verify(x => x.Start("command", "effective arguments"), Times.Once());
            fileExt.Verify(x => x.GetEffectiveDiffArguments(args), Times.Once());
            fileExt.Verify(x => x.GetEffectiveMergeArguments(args), Times.Never());
        }

        [Test]
        public void Merge()
        {
            var args = new[] { "MERGE", "path", "path2", "left label", "right label" };

            var fileExt = new Mock<FileExtension>();
            fileExt.Setup(x => x.IsForExtension(It.IsAny<IEnumerable<string>>())).Returns(true);
            fileExt.Setup(x => x.GetEffectiveMergeArguments(It.IsAny<IList<string>>())).Returns("effective arguments");
            fileExt.Setup(x => x.Command).Returns("command");

            var fileExtLocator = new Mock<IFileExtensionLocator>();
            fileExtLocator.Setup(x => x.GetFileExtension(It.IsAny<IList<string>>())).Returns(fileExt.Object);

            var processExecMock = new Mock<IProcessExecuter>();
            var parser = new CommandLineParser(processExecMock.Object, Mock.Of<IFormDisplayer>(), fileExtLocator.Object);
            parser.Parse(args);


            processExecMock.Verify(x => x.Start("command", "effective arguments"), Times.Once());
            fileExt.Verify(x => x.GetEffectiveMergeArguments(args), Times.Once());
            fileExt.Verify(x => x.GetEffectiveDiffArguments(args), Times.Never());
        }
    }
}