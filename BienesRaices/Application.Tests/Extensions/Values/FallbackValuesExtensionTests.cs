using Application.Extensions.Values;

namespace Application.Tests.Extensions.Values
{
    [TestFixture]
    public class FallbackValuesExtensionTests
    {
        [Test]
        public void FallbackIfEmpty_ReturnsFallbackValueWhenStringIsEmpty()
        {
            // Arrange
            var str = "";
            var fallbackValue = "fallback";

            // Act
            var result = str.FallbackValue(fallbackValue);

            // Assert
            Assert.That(fallbackValue, Is.EqualTo(result));
        }

        [Test]
        public void FallbackIfEmpty_ReturnsOriginalValueWhenStringIsNotEmpty()
        {
            // Arrange
            var str = "original";
            var fallbackValue = "fallback";

            // Act
            var result = str.FallbackValue(fallbackValue);

            // Assert
            Assert.That(str, Is.EqualTo(result));
        }
    }
}
