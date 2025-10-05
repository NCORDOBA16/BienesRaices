using Application.Contracts.Persistence.Common.BaseRepository;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Contracts.Persistence.Common.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
        Task<IDbContextTransaction> GetTransaction();
        IExecutionStrategy CreateExecutionStrategy();
        bool TransactionIsClosed();
        bool TransactionIsOpen();
        void SetConnectionString(string connectionString);
    }
}
