using System.Security.Claims;
using Application.Contracts.Services.AuthServices;
using Application.Exceptions;
using Infrastructure.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Infrastructure.Tests.Services.AuthServices
{
    [TestFixture]
    public class AuthenticatedUserServiceTests
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private AuthenticatedUserService _authenticatedUserService;

        [SetUp]
        public void SetUp()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _authenticatedUserService = new AuthenticatedUserService(_httpContextAccessorMock.Object);
        }

        [Test]
        public void AuthenticatedUserService_ImplementsIAuthenticatedUserService()
        {
            // Assert
            Assert.That(_authenticatedUserService, Is.InstanceOf<IAuthenticatedUserService>());
        }

        [Test]
        public void GetUsernameFromClaims_WhenCalled_ReturnsUserName()
        {
            // Arrange
            var claims = new List<Claim> { new("userName", "TestUser") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(principal);

            // Act
            var result = _authenticatedUserService.GetUsernameFromClaims();

            // Assert
            Assert.That(result, Is.EqualTo("TestUser"));
        }

        [Test]
        public void GetUsernameFromClaims_WhenUserNameIsNotPresent_ThrowsApiException()
        {
            // Arrange
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(principal);

            // Act & Assert
            Assert.Throws<ApiException>(() => _authenticatedUserService.GetUsernameFromClaims());
        }

        [Test]
        public void GetAuthorizationHeader_ShouldReturnAuthorizationHeader_WhenHeaderIsPresent()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "bearer token";
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

            // Act
            var result = _authenticatedUserService.GetAuthorizationHeader();

            // Assert
            Assert.That(result, Is.EqualTo("token"));
        }

        [Test]
        public void GetAuthorizationHeader_ShouldThrowApiException_WhenHeaderIsNotPresent()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

            // Act & Assert
            var ex = Assert.Throws<ApiException>(() => _authenticatedUserService.GetAuthorizationHeader());
            Assert.That(ex!.Message, Is.EqualTo("Not authorized"));
        }

        [Test]
        public void GetAuthorizationHeader_ShouldThrowApiException_WhenHttpContextIsNull()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

            // Act & Assert
            var ex = Assert.Throws<ApiException>(() => _authenticatedUserService.GetAuthorizationHeader());
            Assert.That(ex!.Message, Is.EqualTo("Not authorized"));
        }
    }
}
