using Application.Statics.Configurations;

namespace Application.Tests.Statics.Configurations
{
    [TestFixture]
    public class ExternalApiSettingsTests
    {
        [SetUp]
        public void SetUp()
        {
            ExternalApiSettings.ClientName = "test_client";
        }

        [TearDown]
        public void TearDown()
        {
            ExternalApiSettings.ClientName = string.Empty;
        }

        [Test]
        public void ClientName_WhenSetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange & Act
            var actualClientName = ExternalApiSettings.ClientName;

            // Assert
            Assert.That(actualClientName, Is.EqualTo("test_client"));
        }
    }
}
