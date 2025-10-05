using Application.Features.RandomNumbers.Queries.GenerateRandomNumber;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BienesRaicesAPI.Controllers.v1;

namespace BienesRaicesAPI.Tests.Controllers.V1
{
    [TestFixture]
    public class RandomNumberControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private RandomNumberController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new RandomNumberController(_mediatorMock.Object);
        }

        [Test]
        public async Task GenerateRandomNumber_ReturnsOkResult_WithRandomNumber()
        {
            // Arrange
            var expectedNumber = 42;
            var response = new WrapperResponse<int>(expectedNumber);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GenerateRandomNumberQuery>(), default))
                .ReturnsAsync(response);
            // Act
            var result = await _controller.GenerateRandomNumber();
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
