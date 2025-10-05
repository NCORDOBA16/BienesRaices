using System.Globalization;
using Application.Exceptions;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class ApiExceptionTests
    {
        [Test]
        public void Constructor_Default_ShouldSetDefaultMessage()
        {
            // Act
            var exception = new ApiException();

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var message = "Test message";

            // Act
            var exception = new ApiException(message);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Constructor_WithMessageAndArgs_ShouldFormatMessage()
        {
            // Arrange
            var message = "Test message with args: {0}, {1}";
            var arg1 = "arg1";
            var arg2 = "arg2";
            var expectedMessage = string.Format(CultureInfo.CurrentCulture, message, arg1, arg2);

            // Act
            var exception = new ApiException(message, arg1, arg2);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }
    }
}
