using Application.Behaviours;
using FluentValidation;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Application.Tests.Behaviours
{
    public class ValidationDummyRequest : IRequest<string> { }

    public class ValidationBehaviourTests
    {

        [Test]
        public async Task Handle_NoValidators_CallsNext()
        {
            var behaviour = new ValidationBehaviour<ValidationDummyRequest, string>(Enumerable.Empty<IValidator<ValidationDummyRequest>>());

            var called = false;
            Task<string> Next() { called = true; return Task.FromResult("ok"); }

            var result = await behaviour.Handle(new ValidationDummyRequest(), Next, default);

            Assert.That(called, Is.True);
            Assert.That(result, Is.EqualTo("ok"));
        }

        [Test]
        public void Handle_WithFailures_ThrowsValidationException()
        {
            var validatorMock = new Mock<IValidator<ValidationDummyRequest>>();
            var failure = new FluentValidation.Results.ValidationFailure("Prop", "err");
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<ValidationDummyRequest>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { failure }));

            var behaviour = new ValidationBehaviour<ValidationDummyRequest, string>(new[] { validatorMock.Object });

            Task<string> Next() => Task.FromResult("ok");

            Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await behaviour.Handle(new ValidationDummyRequest(), Next, default));
        }
    }
}
 
