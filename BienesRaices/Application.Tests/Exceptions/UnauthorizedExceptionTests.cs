using Application.Exceptions;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class UnauthorizedExceptionTests
    {
        [Test]
        public void Constructor_Default_ShouldSetDefaultMessage()
        {
            // Arrange
            var expectedMessage = UnauthorizedException.DefaultMessage;

            // Act
            var exception = new UnauthorizedException();

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void Constructor_WithCustomMessage_ShouldCustomMessage()
        {
            // Arrange
            var expectedMessage = "Custom";

            // Act
            var exception = new UnauthorizedException(expectedMessage);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

    }
}
