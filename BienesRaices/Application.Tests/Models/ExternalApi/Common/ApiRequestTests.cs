using Application.Models.ExternalApi.Common;
using Application.Statics.Enums;

namespace Application.Tests.Models.ExternalApi.Common
{
    [TestFixture]
    public class ApiRequestTests
    {
        [Test]
        public void ApiRequest_ShouldHaveProperties()
        {
            // Arrange & Act
            var request = new ApiRequest();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(request, Has.Property("ApiRequestType"));
                Assert.That(request, Has.Property("Url"));
                Assert.That(request, Has.Property("Data"));
                Assert.That(request, Has.Property("AccessToken"));
            });
        }

        [Test]
        public void ApiRequest_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var request = new ApiRequest
            {
                ApiRequestType = ApiRequestType.Get,
                Url = "https://example.com",
                Data = new { Id = 1 },
                AccessToken = "token"
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(request.ApiRequestType, Is.EqualTo(ApiRequestType.Get));
                Assert.That(request.Url, Is.EqualTo("https://example.com"));
                Assert.That(request.Data, Is.EqualTo(new { Id = 1 }));
                Assert.That(request.AccessToken, Is.EqualTo("token"));
            });
        }
    }
}
