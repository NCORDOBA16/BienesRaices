using System.Net;
using System.Reflection;
using Application.Contracts.Services.ExternalApiServices;
using Application.Models.ExternalApi.Common;
using Application.Statics.Configurations;
using Application.Statics.Enums;
using Infrastructure.Services.ExternalApiServices;
using Moq;
using Moq.Protected;

namespace Infrastructure.Tests.Services.ExternalApiServices
{
    [TestFixture]
    public class BaseApiServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private BaseApiService _baseApiService;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _baseApiService = new BaseApiService(_httpClientFactoryMock.Object);
            ExternalApiSettings.ClientName = "TestClient";
        }

        [TearDown]
        public void TearDown()
        {
            ExternalApiSettings.ClientName = string.Empty;
            _baseApiService.Dispose();
        }

        [Test]
        public void BaseApiService_ImplementsIBaseApiService()
        {
            // Assert
            Assert.That(_baseApiService, Is.InstanceOf<IBaseApiService>());
        }

        [Test]
        public async Task SendAsync_WhenCalledWithValidRequest_ReturnsApiResponse()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"result\":\"test\"}")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.GetType(), Is.EqualTo(typeof(ApiResponse)));
                Assert.That(result.Result, Is.EqualTo("test"));
            });
        }

        [Test]
        public async Task SendAsync_WhenCalledAndReponseMessageHasNotSuccessStatusCode_ReturnsApiResponseWithError()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("{\"result\":\"test\"}")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.GetType(), Is.EqualTo(typeof(ApiResponse)));
                Assert.That(result!.IsSuccess, Is.False);
                Assert.That(result!.Result, Is.Null);
                Assert.That(result.DisplayMessage, Is.EqualTo("Error"));
                Assert.That(result.ErrorMessages, Is.Not.Null);
                Assert.That(result.ErrorMessages, Has.Count.AtLeast(1));
            });
        }

        [Test]
        public async Task SendAsync_WhenCalledAndThrowsException_ReturnsApiResponseWithError()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception());

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Result, Is.Null);
                Assert.That(result.DisplayMessage, Is.EqualTo("Error"));
                Assert.That(result.ErrorMessages, Is.Not.Null);
                Assert.That(result.ErrorMessages, Has.Count.AtLeast(1));
                Assert.That(result.IsSuccess, Is.False);
            });
        }

        [Test]
        public async Task SendAsync_WhenCalledAndResponseMessageContentIsNull_ReturnsNull()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = null
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task SendAsync_WhenCalledWithEmptyStringContent_ReturnsNull()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task SendAsync_WhenCalledWithValidToken_ReturnsApiResponse()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get,
                AccessToken = "testToken"
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"result\":\"test\"}")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Result, Is.EqualTo("test"));
            });

        }

        [Test]
        public async Task SendAsync_WhenCalledWithValidTokenAndResponseMessageContentNull_ReturnsNull()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get,
                AccessToken = "testToken"
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = null
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task SendAsync_WhenCalledWithInvalidToken_ReturnsApiResponse()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get,
                AccessToken = "invalidToken"
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{\"result\":\"test\"}")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Result, Is.Null);
                Assert.That(result.DisplayMessage, Is.EqualTo("Error"));
                Assert.That(result.ErrorMessages, Is.Not.Null);
                Assert.That(result.ErrorMessages, Has.Count.AtLeast(1));
                Assert.That(result.IsSuccess, Is.False);
            });
        }

        [Test]
        public async Task SendAsync_WhenCalledWithInvalidToken_ReturnsApiResponseWithException()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get,
                AccessToken = "invalidToken"
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception());

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Result, Is.Null);
                Assert.That(result.DisplayMessage, Is.EqualTo("Error"));
                Assert.That(result.IsSuccess, Is.False);
            });
        }

        [Test]
        public async Task SendAsync_HttpResponseMessageIsNotSuccessStatusCode_ThrowsExternalApiException()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com",
                ApiRequestType = ApiRequestType.Get
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("{\"result\":\"test\"}")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(nameof(BaseApiService.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var response = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response!.IsSuccess, Is.False);
                Assert.That(response.Result, Is.Null);
                Assert.That(response.DisplayMessage, Is.EqualTo("Error"));
                Assert.That(response.ErrorMessages, Is.Not.Null);
            });
            Assert.That(response.ErrorMessages, Has.Count.AtLeast(1));
        }

        [Test]
        public void CreateMessage_WhenCalledWithValidRequest_ReturnsHttpRequestMessage()
        {
            // Arrange
            var apiRequest = new ApiRequest
            {
                Url = "http://test.com/",
                ApiRequestType = ApiRequestType.Get,
                Data = new { Name = "Test" }
            };

            // Act
            var methodInfo = typeof(BaseApiService).GetMethod("CreateMessage", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (HttpRequestMessage?)methodInfo!.Invoke(null, [apiRequest]);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.RequestUri, Is.Not.Null);
                Assert.That(result.Content, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(result.RequestUri.ToString(), Is.EqualTo(apiRequest.Url));
                Assert.That(result.Method, Is.EqualTo(HttpMethod.Get));
                Assert.That(result.Headers.Accept.ToString(), Is.EqualTo("application/json"));
                Assert.That(result.Content.ReadAsStringAsync().Result, Is.EqualTo("{\"Name\":\"Test\"}"));
            });
        }

        [Test]
        public void GetHttpMethod_WhenCalledWithValidApiRequestType_ReturnsCorrectHttpMethod()
        {
            // Arrange
            var methodInfo = typeof(BaseApiService).GetMethod("GetHttpMethod", BindingFlags.NonPublic | BindingFlags.Static)!;

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That((HttpMethod)methodInfo.Invoke(null, [ApiRequestType.Get])!, Is.EqualTo(HttpMethod.Get));
                Assert.That((HttpMethod)methodInfo.Invoke(null, [ApiRequestType.Post])!, Is.EqualTo(HttpMethod.Post));
                Assert.That((HttpMethod)methodInfo.Invoke(null, [ApiRequestType.Put])!, Is.EqualTo(HttpMethod.Put));
                Assert.That((HttpMethod)methodInfo.Invoke(null, [ApiRequestType.Delete])!, Is.EqualTo(HttpMethod.Delete));
            });
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes_WithoutThrowingException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _baseApiService.Dispose();
                _baseApiService.Dispose();
            });
        }

        [Test]
        public void Dispose_SetsDisposedFlag_ToTrue()
        {
            // Act
            _baseApiService.Dispose();

            // Assert
            var disposedField = typeof(BaseApiService).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);
            var disposedValue = (bool)disposedField!.GetValue(_baseApiService)!;

            Assert.That(disposedValue, Is.True);
        }


        [Test]
        public async Task SendAsync_WhenResponseIs500InternalServerError_ReturnsErrorApiResponse()
        {
            // Arrange
            var apiRequest = new ApiRequest { Url = "http://test.com", ApiRequestType = ApiRequestType.Get };
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = "Internal Server Error"
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _baseApiService.SendAsync<ApiResponse>(apiRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.DisplayMessage, Is.EqualTo("Error"));
                Assert.That(result.ErrorMessages, Has.Exactly(1).Items);
                Assert.That(result.ErrorMessages[0], Does.Contain("Error with status code: '500' - Internal Server Error"));
            });
        }
    }
}
