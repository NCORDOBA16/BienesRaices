using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using BienesRaicesAPI.Middlewares;

namespace BienesRaicesAPI.Tests.Middlewares
{
    [TestFixture]
    public class ErrorHandlerMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock;
        private ErrorHandlerMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new ErrorHandlerMiddleware(_nextMock.Object);
        }

        [Test]
        public async Task Invoke_WhenNextThrowsApiException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new ApiException("Test error"));

            // Act
            await _middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(400));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.EqualTo("Test error"));
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsValidationException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new ValidationException());

            // Act
            await _middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(400));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsFluentValidationException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new FluentValidation.ValidationException("test message"));

            // Act
            await _middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(400));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsDbUpdateException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new DbUpdateException(message: "Test message", It.IsAny<SqlException>()));

            // Act
            await _middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(400));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsNotFoundException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new NotFoundException(name: "Test Entity", key: 1));

            // Act
            await _middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(404));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsNullFileException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new NullFileException());

            // Act
            await _middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(400));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsKeyNotFoundException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new KeyNotFoundException());

            // Act
            await _middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(404));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNextThrowsExternalApiException_HandlesError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(n => n(context)).ThrowsAsync(new ExternalApiException());

            // Act
            await _middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenUnhandledExceptionOccurs_ShouldReturnStatusCode500()
        {
            // Arrange
            var requestDelegateMock = new Mock<RequestDelegate>();
            requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).ThrowsAsync(new Exception("Unhandled exception"));

            var middleware = new ErrorHandlerMiddleware(requestDelegateMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            var response = JsonSerializer.Deserialize<WrapperResponse<string>>(responseBody);

            Assert.That(context.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Response.StatusCode, Is.EqualTo(500));
                Assert.That(response!.Succeeded, Is.False);
                Assert.That(response.Message, Is.Not.Null);
                Assert.That(response.Message, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Invoke_WhenNoExceptionOccurs_ShouldNotModifyHttpResponse()
        {
            // Arrange
            var requestDelegateMock = new Mock<RequestDelegate>();
            requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var middleware = new ErrorHandlerMiddleware(requestDelegateMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();

            Assert.Multiple(() =>
            {
                Assert.That(responseBody, Is.Empty); // Verifica que el cuerpo de la respuesta esté vacío
                Assert.That(context.Response.StatusCode, Is.EqualTo(200)); // Verifica que el StatusCode no se haya modificado
            });
        }
    }
}
