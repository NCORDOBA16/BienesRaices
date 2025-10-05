using Application.Contracts.Persistence.Common.BaseRepository;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities.Common;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.Common.BaseRepository
{
    public class BaseRepository<T>(ApplicationDbContext context)
        : RepositoryBase<T>(context), IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context = context;
    }
}
