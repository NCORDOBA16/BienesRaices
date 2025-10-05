using Application.Wrappers;

namespace Application.Tests.Wrappers
{
    [TestFixture]
    public class PagedWrapperResponseTests
    {
        [Test]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var data = new List<string> { "Test1", "Test2" };
            int pageNumber = 1;
            int pageSize = 10;
            int totalRecords = 50;

            // Act
            var response = new PagedWrapperResponse<List<string>>(data, pageNumber, pageSize, totalRecords);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Data, Is.EqualTo(data));
                Assert.That(response.PageNumber, Is.EqualTo(pageNumber));
                Assert.That(response.PageSize, Is.EqualTo(pageSize));
                Assert.That(response.TotalRecords, Is.EqualTo(totalRecords));
                Assert.That(response.TotalPages, Is.EqualTo(5)); // 50 records / 10 per page = 5 pages
            });
        }

        [Test]
        public void Constructor_WithNotAllowedPageSettings_SetsPropertiesCorrectly()
        {
            // Arrange
            var data = new List<string> { "Test1", "Test2" };
            int pageNumber = 1;
            int pageSize = 0;
            int totalRecords = 0;

            // Act
            var response = new PagedWrapperResponse<List<string>>(data, pageNumber, pageSize, totalRecords);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Data, Is.EqualTo(data));
                Assert.That(response.PageNumber, Is.EqualTo(pageNumber));
                Assert.That(response.PageSize, Is.EqualTo(pageSize));
                Assert.That(response.TotalRecords, Is.EqualTo(totalRecords));
                Assert.That(response.TotalPages, Is.EqualTo(0));
            });
        }

        [Test]
        public void Constructor_WithEmptyBody_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var ctor = new PagedWrapperResponse<List<string>>();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ctor.Data, Is.Null);
                Assert.That(ctor.PageNumber, Is.EqualTo(0));
                Assert.That(ctor.PageSize, Is.EqualTo(0));
                Assert.That(ctor.TotalRecords, Is.EqualTo(0));
                Assert.That(ctor.TotalPages, Is.EqualTo(0));
                Assert.That(ctor.Succeeded, Is.False);
            });
        }

        [Test]
        public void Constructor_WithMessageBody_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var ctor = new PagedWrapperResponse<List<string>>("test_message");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ctor.Data, Is.Null);
                Assert.That(ctor.PageNumber, Is.EqualTo(0));
                Assert.That(ctor.PageSize, Is.EqualTo(0));
                Assert.That(ctor.TotalRecords, Is.EqualTo(0));
                Assert.That(ctor.TotalPages, Is.EqualTo(0));
                Assert.That(ctor.Succeeded, Is.False);
                Assert.That(ctor.Message, Is.EqualTo("test_message"));
            });
        }

        [Test]
        public void Constructor_WithDataAndMessageBody_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var sData = new List<string> { "Test1", "Test2" };
            var ctor = new PagedWrapperResponse<List<string>>(sData, "test_message");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ctor.Data, Is.Not.Null);
                Assert.That(ctor.PageNumber, Is.EqualTo(0));
                Assert.That(ctor.PageSize, Is.EqualTo(0));
                Assert.That(ctor.TotalRecords, Is.EqualTo(0));
                Assert.That(ctor.TotalPages, Is.EqualTo(0));
                Assert.That(ctor.Succeeded, Is.True);
                Assert.That(ctor.Message, Is.EqualTo("test_message"));
            });
        }
    }
}
