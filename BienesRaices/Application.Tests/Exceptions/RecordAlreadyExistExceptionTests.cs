using Application.Exceptions;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class RecordAlreadyExistExceptionTests
    {
        [Test]
        public void Constructor_Default_ShouldSetDefaultMessage()
        {
            // Arrange
            var expectedMessage = "Este registro ya existe";

            // Act
            var exception = new RecordAlreadyExistException();

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void Constructor_WithCustomMessage_ShouldSetCustomMessage()
        {
            // Arrange
            var customMessage = "Registro duplicado encontrado";

            // Act
            var exception = new RecordAlreadyExistException(customMessage);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(customMessage));
        }
    }
}
