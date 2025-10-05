using Application.Statics.Configurations;

namespace Application.Tests.Statics.Configurations
{
    [TestFixture]
    public class ApiAuthSettingsTests
    {
        [SetUp]
        public void SetUp()
        {
            ApiAuthSettings.ApiKey = "test_api_key";
            ApiAuthSettings.CorsPolicyName = "cors_policy_name";
        }

        [TearDown]
        public void TearDown()
        {
            ApiAuthSettings.ApiKey = string.Empty;
            ApiAuthSettings.CorsPolicyName = string.Empty;
        }

        [Test]
        public void ApiKey_WhenSetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange & Act
            var actualApiKey = ApiAuthSettings.ApiKey;

            // Assert
            Assert.That(actualApiKey, Is.EqualTo("test_api_key"));
        }

        [Test]
        public void ApiSecret_WhenSetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange & Act
            var actualApiSecret = ApiAuthSettings.CorsPolicyName;

            // Assert
            Assert.That(actualApiSecret, Is.EqualTo("cors_policy_name"));
        }
    }
}
