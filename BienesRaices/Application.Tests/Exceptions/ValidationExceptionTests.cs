using Application.Exceptions;
using FluentValidation.Results;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class ValidationExceptionTests
    {
        [Test]
        public void Constructor_Default_ShouldSetDefaultMessage()
        {
            // Arrange
            var expectedMessage = "Se produjeron uno o más errores de validación";

            // Act
            var exception = new ValidationException();

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo(expectedMessage));
                Assert.That(exception.Errors, Is.Not.Null);
            });
            Assert.That(exception.Errors, Is.Empty);
        }

        [Test]
        public void Constructor_WithFailures_ShouldSetErrors()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new("Property1", "Error message 1"),
                new("Property2", "Error message 2")
            };
            var expectedErrors = new List<string> { "Error message 1", "Error message 2" };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo("Se produjeron uno o más errores de validación"));
                Assert.That(exception.Errors, Is.Not.Null);
            });
            Assert.That(exception.Errors, Has.Count.EqualTo(expectedErrors.Count));
        }

        [Test]
        public void Constructor_WithEmptyFailures_ShouldSetEmptyErrors()
        {
            // Arrange
            var failures = new List<ValidationFailure>();

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo("Se produjeron uno o más errores de validación"));
                Assert.That(exception.Errors, Is.Not.Null);
            });
            Assert.That(exception.Errors, Is.Empty);
        }
    }
}
