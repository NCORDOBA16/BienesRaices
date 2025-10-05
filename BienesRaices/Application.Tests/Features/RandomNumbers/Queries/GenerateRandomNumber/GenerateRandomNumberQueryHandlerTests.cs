using Application.Features.RandomNumbers.Queries.GenerateRandomNumber;
using Application.Wrappers;

namespace Application.Tests.Features.RandomNumbers.Queries.GenerateRandomNumber
{
    [TestFixture]
    public class GenerateRandomNumberQueryHandlerTests
    {
        private GenerateRandomNumberQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new GenerateRandomNumberQueryHandler();
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnsSuccessfulResponse()
        {
            // Arrange
            var query = new GenerateRandomNumberQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<WrapperResponse<int>>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Succeeded, Is.True);
                Assert.That(result.Errors, Is.Null.Or.Empty);
            });
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnsRandomNumberInCorrectRange()
        {
            // Arrange
            var query = new GenerateRandomNumberQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Data, Is.InRange(1, 100), "El número generado no está en el rango esperado de 1 a 100.");
        }
    }
}
