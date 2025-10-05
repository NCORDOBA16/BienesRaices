using Application.Features.RandomNumbers.Queries.GenerateRandomNumber;

namespace Application.Tests.Features.RandomNumbers.Queries.GenerateRandomNumber
{
    [TestFixture]
    public class GenerateRandomNumberQueryTests
    {
        [Test]
        public void Query_IsBuilt_Correctly()
        {
            // Arrange
            // Act
            var query = new GenerateRandomNumberQuery();
            // Assert
            Assert.That(query, Is.Not.Null);
        }
    }
}
