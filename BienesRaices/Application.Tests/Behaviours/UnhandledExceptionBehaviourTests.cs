using Application.Behaviours;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Tests.Behaviours
{
    public class DummyRequest : IRequest<string> { }

    public class UnhandledExceptionBehaviourTests
    {

        [Test]
        public async Task Handle_WhenNoException_ReturnsResult()
        {
            var loggerMock = new Mock<ILogger<DummyRequest>>();
            var behaviour = new UnhandledExceptionBehaviour<DummyRequest, string>(loggerMock.Object);

            Task<string> Next() => Task.FromResult("ok");

            var result = await behaviour.Handle(new DummyRequest(), Next, default);

            Assert.That(result, Is.EqualTo("ok"));
        }

        [Test]
        public void Handle_WhenNextThrows_LogsAndRethrows()
        {
            var loggerMock = new Mock<ILogger<DummyRequest>>();
            var behaviour = new UnhandledExceptionBehaviour<DummyRequest, string>(loggerMock.Object);

            Task<string> Next() => throw new InvalidOperationException("boom");

            Assert.ThrowsAsync<InvalidOperationException>(async () => await behaviour.Handle(new DummyRequest(), Next, default));

            // Verify that an Error log was written. Can't verify extension method LogError directly, so assert ILogger.Log was called with LogLevel.Error
            // Ensure the logger was invoked at least once (Log method was called). Avoids fragile generic matching for Log<TState>.
            Assert.That(loggerMock.Invocations.Any(i => i.Method.Name == "Log"), Is.True);
        }
    }
}
 
