using Ardalis.Specification;
using Domain.Entities.Common;

namespace Application.Contracts.Persistence.Common.BaseRepository
{
    public interface IBaseRepository<T> : IRepositoryBase<T> where T : BaseEntity
    {
    }

    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : BaseEntity
    {
    }
}
