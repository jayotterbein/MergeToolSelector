using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MergeToolSelector.FileExtensions;
using MergeToolSelector.Settings;
using MergeToolSelector.Utility;
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
            var messageDisplayer = new Mock<IMessageDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, messageDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new string[] {});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            messageDisplayer.Verify(x => x.Display(It.IsAny<string>()), Times.Exactly(1));
        }

        [Test]
        public void Help_arg_displays_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var messageDisplayer = new Mock<IMessageDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, messageDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new[] {"hElp"});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            messageDisplayer.Verify(x => x.Display(It.IsAny<string>()), Times.Exactly(1));
        }

        [Test]
        public void Question_arg_displays_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var messageDisplayer = new Mock<IMessageDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, messageDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new[] {"?"});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            messageDisplayer.Verify(x => x.Display(It.IsAny<string>()), Times.Exactly(1));
        }


        [Test]
        public void Unrecognized_command_gives_help()
        {
            var processExecMock = new Mock<IProcessExecuter>();
            var messageDisplayer = new Mock<IMessageDisplayer>();
            var parser = new CommandLineParser(processExecMock.Object, messageDisplayer.Object, Mock.Of<IFileExtensionLocator>());
            parser.Parse(new[] {"nothing"});

            processExecMock.Verify(x => x.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            messageDisplayer.Verify(x => x.Display(It.IsAny<string>()), Times.Once());
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
            var parser = new CommandLineParser(processExecMock.Object, Mock.Of<IMessageDisplayer>(), fileExtLocator.Object);
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
            var parser = new CommandLineParser(processExecMock.Object, Mock.Of<IMessageDisplayer>(), fileExtLocator.Object);
            parser.Parse(args);


            processExecMock.Verify(x => x.Start("command", "effective arguments"), Times.Once());
            fileExt.Verify(x => x.GetEffectiveMergeArguments(args), Times.Once());
            fileExt.Verify(x => x.GetEffectiveDiffArguments(args), Times.Never());
        }
    }
}