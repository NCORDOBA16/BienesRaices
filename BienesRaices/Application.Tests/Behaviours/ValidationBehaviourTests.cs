using Application.Behaviours;
using Application.Tests.Fixtures.Behaviours;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Application.Tests.Behaviours
{
    [TestFixture]
    public class ValidationBehaviourTests
    {
        [Test]
        public void Handle_ThrowsExceptionWhenValidationFails()
        {
            // Arrange
            var mockValidator = new Mock<IValidator<MyValidationBehaviourRequest>>();
            var validationFailure = new ValidationFailure("Property", "Error");
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<MyValidationBehaviourRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { validationFailure }));
            var behaviour = new ValidationBehaviour<MyValidationBehaviourRequest, MyValidationBehaviourResponse>([mockValidator.Object]);
            var request = new MyValidationBehaviourRequest();

            static Task<MyValidationBehaviourResponse> next() => Task.FromResult(new MyValidationBehaviourResponse());

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(() => behaviour.Handle(request, next, new CancellationToken()));
        }

        [Test]
        public async Task Handle_CallsNextWhenValidationSucceeds()
        {
            // Arrange
            var mockValidator = new Mock<IValidator<MyValidationBehaviourRequest>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<MyValidationBehaviourRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            var behaviour = new ValidationBehaviour<MyValidationBehaviourRequest, MyValidationBehaviourResponse>([mockValidator.Object]);
            var request = new MyValidationBehaviourRequest();
            var wasNextCalled = false;
            Task<MyValidationBehaviourResponse> next()
            {
                wasNextCalled = true;
                return Task.FromResult(new MyValidationBehaviourResponse());
            }

            // Act
            await behaviour.Handle(request, next, new CancellationToken());

            // Assert
            Assert.That(wasNextCalled, Is.True);
        }
    }
}
