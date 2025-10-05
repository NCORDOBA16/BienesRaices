using Application.Features.Properties.Commands.CreateProperty;
using Application.DTOs.Properties;
using AutoMapper;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Persistence.Common.BaseRepository;
using System.Threading.Tasks;

namespace Application.Tests.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private IMapper _mapper = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Application.Mappings.Profiles.PropertyProfile).Assembly));
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task Handler_Should_Create_Property_When_Valid()
        {
            var ownerRepoMock = new Mock<IBaseRepository<Owner>>();
           ownerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Owner { IdOwner = Guid.NewGuid() });

            var propertyRepoMock = new Mock<IBaseRepository<Property>>();
           propertyRepoMock.Setup(r =>
        r.FirstOrDefaultAsync(
            It.IsAny<Ardalis.Specification.ISpecification<Property>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);
          propertyRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property p, CancellationToken _) => p) // ✅ Devuelve la misma entidad
            .Verifiable();

            _unitOfWorkMock.Setup(u => u.Repository<Owner>()).Returns(ownerRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propertyRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(1);

            var handler = new CreatePropertyCommandHandler(_unitOfWorkMock.Object, _mapper);

            var command = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "Test", Price = 100, CodeInternal = "C1", IdOwner = Guid.NewGuid() } };

            var result = await handler.Handle(command, default);

            Assert.That(result, Is.Not.Null);
            propertyRepoMock.Verify(
                r => r.AddAsync(
                    It.IsAny<Property>(),
                    It.IsAny<CancellationToken>() // 👈 así compila
                ),
                Times.Once
);
        }
    }
}
