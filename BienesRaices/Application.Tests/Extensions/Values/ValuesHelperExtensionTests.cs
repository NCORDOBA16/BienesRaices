using Application.Extensions.Values;

namespace Application.Tests.Extensions.Values
{
    [TestFixture]
    public class ValuesHelperExtensionTests
    {
        [Test]
        public void ValueToOneItemList_ReturnsCorrectList()
        {
            // Arrange
            var value = "test";

            // Act
            var result = value.ValueToOneItemList();

            // Assert
            Assert.That(new List<string> { "test" }, Is.EqualTo(result));
        }

        [Test]
        public void ThrowIfNullOrEmpty_ThrowsExceptionWhenStringIsEmpty()
        {
            // Arrange
            var str = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => str.ThrowIfNullOrEmpty());
        }

        [Test]
        public void ThrowIfNullOrEmpty_ReturnsOriginalValueWhenStringIsNotEmpty()
        {
            // Arrange
            var str = "original";

            // Act
            var result = str.ThrowIfNullOrEmpty();

            // Assert
            Assert.That(str, Is.EqualTo(result));
        }
    }
}
