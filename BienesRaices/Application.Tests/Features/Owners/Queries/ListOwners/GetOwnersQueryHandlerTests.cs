using Application.Features.Owners.Queries.ListOwners;
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

namespace Application.Tests.Features.Owners.Queries.ListOwners
{
    public class GetOwnersQueryHandlerTests
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
            var list = new List<Owner> { new Owner { IdOwner = Guid.NewGuid(), Name = "O1", Address = "A1" } };

            var repoMock = TestFixtures.CreateRepoMock<Owner>();
            repoMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);

            _unitOfWorkMock.Setup(u => u.Repository<Owner>()).Returns(repoMock.Object);

            var handler = new GetOwnersQueryHandler(_unitOfWorkMock.Object, _mapper);
            var query = new GetOwnersQuery();

            var result = await handler.Handle(query, default);

            repoMock.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Has.Exactly(1).Items);
        }
    }
}
 
