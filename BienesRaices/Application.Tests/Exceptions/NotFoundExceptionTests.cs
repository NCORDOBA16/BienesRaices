using Application.Exceptions;

namespace Application.Tests.Exceptions
{
    [TestFixture]
    public class NotFoundExceptionTests
    {
        [Test]
        public void Constructor_ShouldSetMessage()
        {
            // Arrange
            var entityName = "TestEntity";
            var key = 123;
            var expectedMessage = $"Entity \"{entityName}\" ({key})  no fue encontrado";

            // Act
            var exception = new NotFoundException(entityName, key);

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }
    }
}
