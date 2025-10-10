using AutoMapper;
using Moq;
using Application.Mappings.Profiles;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Persistence.Common.BaseRepository;
using Domain.Entities.Common;

namespace Application.Tests.Shared
{
    public static class TestFixtures
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(PropertyProfile).Assembly));
            return config.CreateMapper();
        }

        public static Mock<IUnitOfWork> CreateUnitOfWorkMock()
        {
            return new Mock<IUnitOfWork>();
        }

        public static Mock<IBaseRepository<T>> CreateRepoMock<T>() where T : BaseEntity
        {
            return new Mock<IBaseRepository<T>>();
        }
    }
}
