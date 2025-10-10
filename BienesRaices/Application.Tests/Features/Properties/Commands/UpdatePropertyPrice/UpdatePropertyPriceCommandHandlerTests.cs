using Application.Features.Properties.Commands.UpdatePropertyPrice;
using Application.DTOs.Properties;
using AutoMapper;
using Domain.Entities;
using Moq;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Persistence.Common.BaseRepository;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using System;
using Application.Tests.Shared;

namespace Application.Tests.Features.Properties.Commands.UpdatePropertyPrice
{
    public class UpdatePropertyPriceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private IMapper _mapper = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = TestFixtures.CreateUnitOfWorkMock();
            _mapper = TestFixtures.CreateMapper();
        }

        [Test]
        public async Task Handler_Should_Update_Price_And_Add_Trace_When_Valid()
        {
            var property = new Property { IdProperty = Guid.NewGuid(), Name = "P", Price = 50, CodeInternal = "C1" };

            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(property);
            propRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>())).ReturnsAsync(0).Verifiable();

            var traceRepoMock = TestFixtures.CreateRepoMock<PropertyTrace>();
            traceRepoMock.Setup(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>())).ReturnsAsync((PropertyTrace t, CancellationToken _) => t).Verifiable();

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<PropertyTrace>()).Returns(traceRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            var handler = new UpdatePropertyPriceCommandHandler(_unitOfWorkMock.Object, _mapper);

            var command = new UpdatePropertyPriceCommand { IdProperty = property.IdProperty, NewPrice = 75m };

            var result = await handler.Handle(command, default);

            propRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()), Times.Once);
            traceRepoMock.Verify(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Price, Is.EqualTo(75m));
        }

        [Test]
        public void Handler_Should_Throw_NotFoundException_When_Property_NotFound()
        {
            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Property?)null);

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);

            var handler = new UpdatePropertyPriceCommandHandler(_unitOfWorkMock.Object, _mapper);
            var command = new UpdatePropertyPriceCommand { IdProperty = Guid.NewGuid(), NewPrice = 1m };

            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
