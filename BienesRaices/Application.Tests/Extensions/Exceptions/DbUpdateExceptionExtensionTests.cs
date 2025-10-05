using Application.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests.Extensions.Exceptions
{
    [TestFixture]
    public class DbUpdateExceptionExtensionTests
    {
        [Test]
        public void GetMessageFromDbUpdateException_GetMainMessage_WhenInnerIsNull()
        {
            // Arrange
            var ex = new DbUpdateException("Error", innerException: null);

            // Act
            var result = ex.GetMessageFromDbUpdateException();

            // Assert
            Assert.That(result, Is.EqualTo($"{DbUpdateExceptionExtension.BaseMessage} {ex.Message}"));
        }

        [Test]
        public void GetMessageFromDbUpdateException_GetInnerMessage_WhenInnerIsNotNull()
        {
            // Arrange
            var ex = new DbUpdateException("Error", innerException: new Exception("Inner Message"));

            // Act
            var result = ex.GetMessageFromDbUpdateException();
            Assert.Multiple(() =>
            {

                // Assert
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(result, Is.EqualTo($"{DbUpdateExceptionExtension.BaseMessage} {ex.InnerException!.Message}"));
            });
        }
    }
}
