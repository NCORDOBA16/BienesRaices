using Application.Attributes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Attributes
{
    [TestFixture]
    public class RegisterExternalServiceAttributeTests
    {
        [Test]
        public void Constructor_SetsLifeTimeProperty()
        {
            // Arrange
            var expectedLifetime = ServiceLifetime.Scoped;

            // Act
            var attribute = new RegisterExternalServiceAttribute(expectedLifetime);

            // Assert
            Assert.That(attribute.LifeTime, Is.EqualTo(expectedLifetime));
        }

        [Test]
        public void BaseAddress_IsCorrect()
        {
            // Arrange
            var expectedBaseAddress = "/";

            // Act
            var actualBaseAddress = RegisterExternalServiceAttribute.BaseAddress;

            // Assert
            Assert.That(actualBaseAddress, Is.EqualTo(expectedBaseAddress));

        }
    }
}
