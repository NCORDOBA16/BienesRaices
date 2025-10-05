using Application.Statics.Configurations;

namespace Application.Tests.Statics.Configurations
{
    [TestFixture]
    public class DbSettingsTests
    {
        [SetUp]
        public void SetUp()
        {
            DbSettings.TimeoutInMinutes = 10;
            DbSettings.DefaultConnection = "test_connection";
        }

        [TearDown]
        public void TearDown()
        {
            DbSettings.TimeoutInMinutes = 0;
            DbSettings.DefaultConnection = string.Empty;
        }

        [Test]
        public void DefaultConnection_WhenSetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange & Act
            var actualDefaultConnection = DbSettings.DefaultConnection;

            // Assert
            Assert.That(actualDefaultConnection, Is.EqualTo("test_connection"));
        }

        [Test]
        public void TimeoutInMinutes_WhenSetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange & Act
            var actualTimeoutInMinutes = DbSettings.TimeoutInMinutes;

            // Assert
            Assert.That(actualTimeoutInMinutes, Is.EqualTo(10));
        }
    }
}
