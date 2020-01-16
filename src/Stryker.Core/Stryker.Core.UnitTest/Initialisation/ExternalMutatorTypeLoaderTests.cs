using System;
using System.IO.Abstractions;
using Moq;
using Stryker.Core.Initialisation;
using Xunit;

namespace Stryker.Core.UnitTest.Initialisation
{
    public class ExternalMutatorTypeLoaderTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void LoadExternalMutators_ShouldThrowArgumentExceptionOnEmptyPath(string path)
        {
            var fileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);

            var target = new ExternalMutatorTypeLoader(fileSystemMock.Object);

            Assert.Throws<ArgumentException>(() => target.LoadExternalMutators(path));
        }
    }
}
