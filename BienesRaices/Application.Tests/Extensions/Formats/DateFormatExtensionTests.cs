using Application.Extensions.Formats;

namespace Application.Tests.Extensions.Formats
{
    [TestFixture]
    public class DateFormatExtensionTests
    {
        [Test]
        public void ExportableDateTimeFormat_ShouldReturnCorrectFormat()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 1, 13, 14, 15, DateTimeKind.Local);
            var expected = "01-01-2024 01-14-15";

            // Act
            var result = dateTime.ExportableDateTimeFormat();

            // Assert
            Assert.That(expected, Is.EqualTo(result));
        }

        [Test]
        public void ExportableDateFormat_ShouldReturnCorrectFormat()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var expected = "01-01-2024";

            // Act
            var result = dateTime.ExportableDateFormat();

            // Assert
            Assert.That(expected, Is.EqualTo(result));
        }
    }
}
