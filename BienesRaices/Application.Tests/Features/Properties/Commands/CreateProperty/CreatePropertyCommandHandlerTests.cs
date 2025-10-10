using Application.Features.Properties.Commands.CreateProperty;
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

namespace Application.Tests.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandlerTests
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
        public async Task Handler_Should_Create_Property_When_Valid()
        {
            var ownerRepoMock = TestFixtures.CreateRepoMock<Owner>();
            ownerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Owner { IdOwner = Guid.NewGuid() });

            var propertyRepoMock = TestFixtures.CreateRepoMock<Property>();
            propertyRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<Property>>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Property?)null);
            propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Property p, CancellationToken _) => p)
                            .Verifiable();

            _unitOfWorkMock.Setup(u => u.Repository<Owner>()).Returns(ownerRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propertyRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            var handler = new CreatePropertyCommandHandler(_unitOfWorkMock.Object, _mapper);

            var command = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "Test", Price = 100, CodeInternal = "C1", IdOwner = Guid.NewGuid() } };

            var result = await handler.Handle(command, default);

            propertyRepoMock.Verify(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            // Validate mapping: PropertyDto contains CodeInternal and PropertyName mapping in profile
            Assert.That(result.Data.PropertyName, Is.EqualTo("Test"));
        }

        [Test]
        public void Handler_Should_Throw_NotFoundException_When_Owner_NotFound()
        {
            var ownerRepoMock = TestFixtures.CreateRepoMock<Owner>();
            ownerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Owner?)null);

            _unitOfWorkMock.Setup(u => u.Repository<Owner>()).Returns(ownerRepoMock.Object);

            var handler = new CreatePropertyCommandHandler(_unitOfWorkMock.Object, _mapper);
            var command = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "T", Price = 1, CodeInternal = "C", IdOwner = Guid.NewGuid() } };

            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, default));
        }

        [Test]
        public void Handler_Should_Throw_RecordAlreadyExistException_When_Duplicate_CodeInternal()
        {
            var ownerRepoMock = TestFixtures.CreateRepoMock<Owner>();
            ownerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Owner { IdOwner = Guid.NewGuid() });

            var propertyRepoMock = TestFixtures.CreateRepoMock<Property>();
            propertyRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Ardalis.Specification.ISpecification<Property>>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new Property { IdProperty = Guid.NewGuid() });

            _unitOfWorkMock.Setup(u => u.Repository<Owner>()).Returns(ownerRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propertyRepoMock.Object);

            var handler = new CreatePropertyCommandHandler(_unitOfWorkMock.Object, _mapper);
            var command = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "T", Price = 1, CodeInternal = "C", IdOwner = Guid.NewGuid() } };

            Assert.ThrowsAsync<RecordAlreadyExistException>(async () => await handler.Handle(command, default));
        }
    }
}
