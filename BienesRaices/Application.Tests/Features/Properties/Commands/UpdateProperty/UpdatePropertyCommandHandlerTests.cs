using Application.Features.Properties.Commands.UpdateProperty;
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

namespace Application.Tests.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandlerTests
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
        public async Task Handler_Should_Update_Property_When_Valid()
        {
            var property = new Property { IdProperty = Guid.NewGuid(), Name = "Old", Address = "Addr", Price = 10, CodeInternal = "C1" };

            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(property);
            propRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>())).ReturnsAsync(0).Verifiable();

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            var handler = new UpdatePropertyCommandHandler(_unitOfWorkMock.Object, _mapper);

            var command = new UpdatePropertyCommand { IdProperty = property.IdProperty, Title = "New", Description = "NewAddr", Price = 20, CodeInternal = "C2", Year = 1999 };

            var result = await handler.Handle(command, default);

            propRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.PropertyName, Is.EqualTo("New"));
            Assert.That(result.Data.Price, Is.EqualTo(20));
        }

        [Test]
        public void Handler_Should_Throw_NotFoundException_When_Property_NotFound()
        {
            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Property?)null);

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);

            var handler = new UpdatePropertyCommandHandler(_unitOfWorkMock.Object, _mapper);
            var command = new UpdatePropertyCommand { IdProperty = Guid.NewGuid(), Title = "T" };

            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
