using Application.Features.Properties.Queries.ListProperties;
using AutoMapper;
using Domain.Entities;
using Moq;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Persistence.Common.BaseRepository;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Tests.Shared;
using System.Collections.Generic;

namespace Application.Tests.Features.Properties.Queries.ListProperties
{
    public class GetPropertiesQueryHandlerTests
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
        public async Task Handler_Should_Return_Mapped_Dtos()
        {
            var list = new List<Property> { new Property { IdProperty = Guid.NewGuid(), Name = "P1", Address = "A1", Price = 10 } };

            var propRepoMock = TestFixtures.CreateRepoMock<Property>();
            propRepoMock.Setup(r => r.ListAsync(It.IsAny<Ardalis.Specification.ISpecification<Property>>(), It.IsAny<CancellationToken>())).ReturnsAsync(list);

            _unitOfWorkMock.Setup(u => u.Repository<Property>()).Returns(propRepoMock.Object);

            var handler = new GetPropertiesQueryHandler(_unitOfWorkMock.Object, _mapper);
            var query = new GetPropertiesQuery();

            var result = await handler.Handle(query, default);

            propRepoMock.Verify(r => r.ListAsync(It.IsAny<Ardalis.Specification.ISpecification<Property>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PropertyName, Is.EqualTo("P1"));
        }
    }
}
