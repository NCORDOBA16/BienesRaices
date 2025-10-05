using Application.Models.ExternalApi.Common;

namespace Application.Tests.Models.ExternalApi.Common
{
    [TestFixture]
    public class ApiResponseTests
    {
        [Test]
        public void ApiResponse_ShouldHaveProperties()
        {
            // Arrange & Act
            var response = new ApiResponse();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Has.Property("IsSuccess"));
                Assert.That(response, Has.Property("Result"));
                Assert.That(response, Has.Property("DisplayMessage"));
                Assert.That(response, Has.Property("ErrorMessages"));
            });
        }

        [Test]
        public void ApiResponse_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var response = new ApiResponse
            {
                IsSuccess = false,
                Result = new { Id = 1 },
                DisplayMessage = "message",
                ErrorMessages = ["error"]
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccess, Is.False);
                Assert.That(response.Result, Is.EqualTo(new { Id = 1 }));
                Assert.That(response.DisplayMessage, Is.EqualTo("message"));
                Assert.That(response.ErrorMessages, Is.EqualTo(new List<string> { "error" }));
            });
        }
    }
}
