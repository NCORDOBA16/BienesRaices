using System.Globalization;
using Application.Exceptions;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class ExternalApiExceptionTests
    {
        [Test]
        public void Constructor_Default_ShouldSetDefaultValues()
        {
            // Act
            var exception = new ExternalApiException();

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.IsSuccess, Is.True);
                Assert.That(exception.Result, Is.Null);
                Assert.That(exception.ErrorMessages, Is.Not.Null);
            });
            Assert.That(exception.ErrorMessages, Is.Empty);
        }

        [Test]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var message = "Test message";

            // Act
            var exception = new ExternalApiException(message);

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
            var exception = new ExternalApiException(message, arg1, arg2);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetOneLineMessage_ShouldReturnFormattedMessage()
        {
            // Arrange
            var exception = new ExternalApiException
            {
                ErrorMessages = new List<string> { "Error1", "Error2", "Error3" }
            };
            var expectedMessage = "Error1 --- Error2 --- Error3";

            // Act
            var result = exception.GetOneLineMessage();

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetOneLineMessage_ShouldReturnEmptyString_WhenNoErrorMessages()
        {
            // Arrange
            var exception = new ExternalApiException();

            // Act
            var result = exception.GetOneLineMessage();

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetOneLineMessage_ShouldReturnEmptyString_WhenErrorMessagesAreNullOrEmpty()
        {
            // Arrange
#nullable disable
            var exception = new ExternalApiException
            {
                ErrorMessages = new List<string> { null, string.Empty }
            };
#nullable enable

            // Act
            var result = exception.GetOneLineMessage();

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}
