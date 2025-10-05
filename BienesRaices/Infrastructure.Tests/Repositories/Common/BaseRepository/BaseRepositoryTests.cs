using Domain.Entities.Common;
using Infrastructure.DbContexts;
using Infrastructure.Repositories.Common.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Common.BaseRepository
{
    // 1. Definimos una entidad ficticia para usar en las pruebas genéricas.
    public class DummyEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // 2. Creamos un DbContext específico para la prueba que incluye la DummyEntity.
    public class TestDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
    {
        public DbSet<DummyEntity> DummyEntities { get; set; }
    }


    [TestFixture]
    public class BaseRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            // 3. Configuramos la base de datos en memoria para nuestro TestDbContext.
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{TestContext.CurrentContext.Test.Name}")
                .Options;
        }

        [Test]
        public void Constructor_Should_Initialize_Repository_Successfully()
        {
            // Arrange
            // Usamos el TestDbContext que conoce a DummyEntity.
            using var context = new TestDbContext(_dbContextOptions);

            // Act
            var repository = new BaseRepository<DummyEntity>(context);

            // Assert
            Assert.That(repository, Is.Not.Null);
        }

        [Test]
        public async Task AddAsync_Should_Add_Entity_To_Context()
        {
            // Arrange
            var entity = new DummyEntity { Id = 1, Name = "Test" };
            // Usamos el TestDbContext en todas las instancias.
            await using var context = new TestDbContext(_dbContextOptions);
            var repository = new BaseRepository<DummyEntity>(context);

            // Act
            await repository.AddAsync(entity);
            await context.SaveChangesAsync();

            // Assert
            // Verificamos en una nueva instancia del TestDbContext.
            await using var assertContext = new TestDbContext(_dbContextOptions);
            var addedEntity = await assertContext.DummyEntities.FindAsync(entity.Id);

            Assert.That(addedEntity, Is.Not.Null);
            Assert.That(addedEntity.Name, Is.EqualTo(entity.Name));
        }
    }
}