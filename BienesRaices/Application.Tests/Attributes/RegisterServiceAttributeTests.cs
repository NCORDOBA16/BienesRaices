using Application.Attributes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Attributes
{
    [TestFixture]
    public class RegisterServiceAttributeTests
    {
        [Test]
        public void Constructor_SetsLifeTimeProperty()
        {
            // Arrange
            var expectedLifetime = ServiceLifetime.Scoped;

            // Act
            var attribute = new RegisterServiceAttribute(expectedLifetime);

            // Assert
            Assert.That(attribute.LifeTime, Is.EqualTo(expectedLifetime));
        }
    }
}
