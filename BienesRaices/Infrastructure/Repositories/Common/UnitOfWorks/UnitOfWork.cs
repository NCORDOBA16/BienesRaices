using System.Collections;
using System.Data;
using Application.Attributes.Services;
using Application.Contracts.Persistence.Common.BaseRepository;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Exceptions;
using Domain.Entities.Common;
using Infrastructure.DbContexts;
using Infrastructure.Repositories.Common.BaseRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories.Common.UnitOfWorks
{
    [RegisterService(ServiceLifetime.Scoped)]
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private bool _disposed = false;
        private Hashtable _repositories = [];
        private readonly ApplicationDbContext _context = context;

        public async Task<int> Complete()
        {
            try
            {
                _context.ChangeTracker.DetectChanges();
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApiException("Error writing to data container");
            }
        }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            _repositories ??= [];

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IBaseRepository<TEntity>)_repositories[type]!;
        }

        public Task<IDbContextTransaction> GetTransaction()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _context.Database.CreateExecutionStrategy();
        }

        public void SetConnectionString(string connectionString)
        {
            _context.Database.SetConnectionString(connectionString);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool TransactionIsClosed()
        {
            return _context.Database.CurrentTransaction is null || _context.Database.CurrentTransaction.GetDbTransaction().Connection?.State == ConnectionState.Closed;
        }
        public bool TransactionIsOpen()
        {
            return _context.Database.CurrentTransaction is not null && _context.Database.CurrentTransaction.GetDbTransaction().Connection?.State == ConnectionState.Open;
        }
    }
}
