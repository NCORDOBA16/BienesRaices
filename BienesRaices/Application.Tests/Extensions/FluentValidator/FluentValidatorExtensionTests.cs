using Application.Extensions.FluentValidator;
using Application.Tests.Fixtures.Extensions;

namespace Application.Tests.Extensions.FluentValidator
{
    [TestFixture]
    public class FluentValidatorExtensionTests
    {
        [Test]
        public void EndDateGreaterThanStartDate_ReturnsCorrectResult()
        {
            // Arrange
            var startDate = DateTime.Now.AddMonths(-2);
            var endDate = DateTime.Now;
            var request = new { StartDate = startDate, EndDate = endDate };

            // Act
            var result = request.EndDateGreaterThanStartDate("StartDate", "EndDate");

            // Assert
            Assert.That(result);
        }

        [Test]
        public void EndDateEqualStartDate_ReturnsFalseWhenDatesAreEqual()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = startDate;
            var request = new { StartDate = startDate, EndDate = endDate };

            // Act
            var result = request.EndDateGreaterThanStartDate("StartDate", "EndDate");

            // Assert
            Assert.That(result, Is.False);


        }

        [Test]
        public void EndDateLessThanStartDate_ReturnsFalseWhenDatesAreEqual()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddMonths(-1);
            var request = new { StartDate = startDate, EndDate = endDate };

            // Act
            var result = request.EndDateGreaterThanStartDate("StartDate", "EndDate");

            // Assert
            Assert.That(result, Is.False);


        }

        [Test]
        public void PagingValuesMustBeValid_ReturnsCorrectResult()
        {
            // Arrange
            var request = new MyPagedModel { PageSize = 10, PageNumber = 1 };

            // Act
            var result = request.PagingValuesMustBeValid();

            // Assert
            Assert.That(result);
        }

        [Test]
        public void PagingValuesMustBeValid_ReturnsFalseWhenPageSizeIsLessThanOne()
        {
            // Arrange
            var request = new MyPagedModel { PageSize = 0, PageNumber = 1 };

            // Act
            var result = request.PagingValuesMustBeValid();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void PagingValuesMustBeValid_ReturnsFalseWhenPageNumberIsLessThanOne()
        {
            // Arrange
            var request = new MyPagedModel { PageSize = 1, PageNumber = 0 };

            // Act
            var result = request.PagingValuesMustBeValid();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void PagingValuesMustBeValid_ReturnsFalseWhenPageNumberAndPageSizeAreLessThanOne()
        {
            // Arrange
            var request = new MyPagedModel { PageSize = 0, PageNumber = 0 };

            // Act
            var result = request.PagingValuesMustBeValid();

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
