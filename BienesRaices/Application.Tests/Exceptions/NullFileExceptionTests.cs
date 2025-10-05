using Application.Exceptions;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class NullFileExceptionTests
    {
        [Test]
        public void Constructor_Default_ShouldSetDefaultMessage()
        {
            // Arrange
            var expectedMessage = "La solicitud requiere que seleccione un archivo";

            // Act
            var exception = new NullFileException();

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void Constructor_WithCustomMessage_ShouldSetCustomMessage()
        {
            // Arrange
            var customMessage = "Archivo no encontrado";

            // Act
            var exception = new NullFileException(customMessage);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(customMessage));
        }
    }
}
