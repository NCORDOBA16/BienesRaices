using System.Net;
using System.Text.Json;
using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Moq;
using BienesRaicesAPI.Middlewares;

namespace BienesRaicesAPI.Tests.Middlewares
{
    [TestFixture]
    public class NotFoundMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock;
        private NotFoundMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new NotFoundMiddleware(_nextMock.Object);
        }

        [Test]
        public async Task InvokeAsync_ShouldReturnNotFoundResponse_WhenStatusCodeIs404AndResponseNotStarted()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
            var responseModel = JsonSerializer.Deserialize<WrapperResponse<string>>(responseBody);

            Assert.Multiple(() =>
            {
                Assert.That(context.Response.ContentType, Is.EqualTo("application/json"));
                Assert.That(context.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(responseModel, Is.Not.Null);
                Assert.That(responseModel!.Succeeded, Is.False);
                Assert.That(responseModel.Message, Is.EqualTo("El recurso que estás buscando no existe."));
                Assert.That(responseModel.Errors, Is.Not.Null);
                Assert.That(responseModel.Errors, Has.Count.EqualTo(1));
                Assert.That(responseModel.Errors[0], Is.EqualTo("El recurso que estás buscando no existe."));
            });
        }

        [Test]
        public async Task InvokeAsync_ShouldNotModifyResponse_WhenStatusCodeIsNot404()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(context);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(context.Response.ContentType, Is.Not.EqualTo("application/json"));
                Assert.That(context.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            });
        }

        [Test]
        public async Task InvokeAsync_ShouldNotModifyResponse_WhenResponseHasStarted()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.Headers["TestHeader"] = "TestValue";

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Callback<HttpContext>(ctx =>
            {
                ctx.Response.Headers["TestHeader"] = "TestValue";
            }).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.That(context.Response.Headers["TestHeader"], Is.EqualTo("TestValue"));
        }
    }
}
