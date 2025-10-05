using Application.Behaviours;
using Application.Tests.Fixtures.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Behaviours
{
    [TestFixture]
    public class UnhandledExceptionBehaviourTests
    {
        private Mock<ILogger<MyUnhandledBehaviourRequest>> _loggerMock;
        private UnhandledExceptionBehaviour<MyUnhandledBehaviourRequest, MyUnhandleBehaviourResponse> _behaviour;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<MyUnhandledBehaviourRequest>>();
            _behaviour = new UnhandledExceptionBehaviour<MyUnhandledBehaviourRequest, MyUnhandleBehaviourResponse>(_loggerMock.Object);
        }

        [Test]
        public async Task Handle_WhenNoError_ShouldCallNext()
        {
            // create this test
            // Arrange
            var request = new MyUnhandledBehaviourRequest();
            var expectedResponse = new MyUnhandleBehaviourResponse();
            var cancellationToken = new CancellationToken();
            async Task<MyUnhandleBehaviourResponse> next() => await Task.FromResult(expectedResponse);

            // Act
            var result = await _behaviour.Handle(request, next, cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    null,
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Never); // Verifica que LogError nunca se llama
        }

        [Test]
        public void Handle_WhenExceptionIsThrown_ShouldLogError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MyUnhandledBehaviourRequest>>();
            var behaviour = new UnhandledExceptionBehaviour<MyUnhandledBehaviourRequest, MyUnhandleBehaviourResponse>(mockLogger.Object);
            var request = new MyUnhandledBehaviourRequest();
            var cancellationToken = new CancellationToken();
            var delegateMock = new Mock<RequestHandlerDelegate<MyUnhandleBehaviourResponse>>();
            var exception = new Exception("Test exception");
            delegateMock.Setup(d => d()).ThrowsAsync(exception);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => behaviour.Handle(request, delegateMock.Object, cancellationToken));
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    exception,
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
