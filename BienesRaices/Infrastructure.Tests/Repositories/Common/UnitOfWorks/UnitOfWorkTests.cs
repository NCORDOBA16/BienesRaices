using System.ComponentModel.DataAnnotations;
using Application.Exceptions;
using Domain.Entities.Common;
using Infrastructure.DbContexts;
using Infrastructure.Repositories.Common.UnitOfWorks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Common.UnitOfWorks
{
    // Entidad ficticia para las pruebas
    public class DummyEntity : BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }

    // DbContext de prueba que conoce la entidad ficticia
    public class TestDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
    {
        public DbSet<DummyEntity> DummyEntities { get; set; }
    }

    // DbContext especializado para simular fallos al guardar
    public class FailingDbContext(DbContextOptions<ApplicationDbContext> options) : TestDbContext(options)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new DbUpdateException("Simulated database update error.");
        }
    }

    // DbContext especializado para verificar llamadas a Dispose
    public class VerifiableDbContext(DbContextOptions<ApplicationDbContext> options) : TestDbContext(options)
    {
        public int DisposeCallCount { get; private set; }

        public override void Dispose()
        {
            DisposeCallCount++;
            GC.SuppressFinalize(this);
        }
    }


    [TestFixture]
    public class UnitOfWorkTests
    {
        private DbContextOptions<ApplicationDbContext> _inMemoryOptions;
        private SqliteConnection _sqliteConnection;
        private DbContextOptions<ApplicationDbContext> _sqliteOptions;

        [SetUp]
        public void SetUp()
        {
            // Configuración para proveedor en memoria
            _inMemoryOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"UowTestDb_{TestContext.CurrentContext.Test.Name}")
                .Options;

            // Configuración para proveedor relacional (SQLite)
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();
            _sqliteOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;

            // Asegura que el esquema se cree para las pruebas con SQLite
            using var context = new TestDbContext(_sqliteOptions);
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _sqliteConnection?.Close();
        }

        [Test]
        public void Repository_WhenCalledFirstTime_ShouldCreateAndCacheRepository()
        {
            // Arrange
            using var context = new TestDbContext(_inMemoryOptions);
            using var unitOfWork = new UnitOfWork(context);

            // Act
            var repo1 = unitOfWork.Repository<DummyEntity>();

            // Assert
            Assert.That(repo1, Is.Not.Null);
        }

        [Test]
        public void Repository_WhenCalledMultipleTimes_ShouldReturnSameCachedInstance()
        {
            // Arrange
            using var context = new TestDbContext(_inMemoryOptions);
            using var unitOfWork = new UnitOfWork(context);

            // Act
            var repo1 = unitOfWork.Repository<DummyEntity>();
            var repo2 = unitOfWork.Repository<DummyEntity>();

            // Assert
            Assert.That(repo1, Is.SameAs(repo2));
        }

        [Test]
        public async Task Complete_WhenSuccessful_ShouldSaveChanges()
        {
            // Arrange
            using var context = new TestDbContext(_inMemoryOptions);
            using var unitOfWork = new UnitOfWork(context);

            // TÉCNICA CLAVE: Añadir la entidad directamente al contexto.
            // Esto registra el cambio en el ChangeTracker de una manera que persiste
            // incluso con la configuración global de NoTracking.
            await context.AddAsync(new DummyEntity());

            // Act
            var result = await unitOfWork.Complete();

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Complete_WhenSaveChangesFails_ShouldThrowApiException()
        {
            // Arrange
            // Usamos una implementación de DbContext que siempre falla al guardar.
            using var failingContext = new FailingDbContext(_inMemoryOptions);
            using var unitOfWork = new UnitOfWork(failingContext);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ApiException>(async () => await unitOfWork.Complete());
            Assert.That(ex.Message, Is.EqualTo("Error writing to data container"));
        }

        [Test]
        public async Task TransactionMethods_ShouldBehaveCorrectly_WithRelationalProvider()
        {
            // Arrange
            using var context = new TestDbContext(_sqliteOptions);
            using var unitOfWork = new UnitOfWork(context);

            Assert.Multiple(() =>
            {
                // Act & Assert (Estado inicial)
                Assert.That(unitOfWork.TransactionIsClosed(), Is.True);
                Assert.That(unitOfWork.TransactionIsOpen(), Is.False);
            });

            // Act (Iniciar transacción)
            await using var transaction = await unitOfWork.GetTransaction();

            Assert.Multiple(() =>
            {
                // Assert (Estado con transacción abierta)
                Assert.That(unitOfWork.TransactionIsClosed(), Is.False);
                Assert.That(unitOfWork.TransactionIsOpen(), Is.True);
                Assert.That(transaction, Is.Not.Null);
            });

            // Act (Commit y cierre)
            await transaction.CommitAsync();

            // Assert (Estado final)
            Assert.That(unitOfWork.TransactionIsClosed(), Is.True);
        }

        [Test]
        public void CreateExecutionStrategy_ShouldReturnStrategy()
        {
            // Arrange
            using var context = new TestDbContext(_sqliteOptions);
            using var unitOfWork = new UnitOfWork(context);

            // Act
            var strategy = unitOfWork.CreateExecutionStrategy();

            // Assert
            Assert.That(strategy, Is.Not.Null);
        }

        [Test]
        public void SetConnectionString_ShouldNotThrowException()
        {
            // Arrange
            // Se crea un contexto con una conexión cerrada específicamente para esta prueba.
            var optionsForThisTest = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TestDbContext(optionsForThisTest);
            using var unitOfWork = new UnitOfWork(context);
            var newConnectionString = "DataSource=new_test.db";

            // Act & Assert
            Assert.DoesNotThrow(() => unitOfWork.SetConnectionString(newConnectionString));
        }

        [Test]
        public void Dispose_ShouldDisposeContextAndBeSafeForMultipleCalls()
        {
            // Arrange
            // Se utiliza una implementación real del DbContext diseñada para esta verificación.
            var verifiableContext = new VerifiableDbContext(_inMemoryOptions);
            var unitOfWork = new UnitOfWork(verifiableContext);

            // Act
            unitOfWork.Dispose();
            unitOfWork.Dispose(); // Segunda llamada

            // Assert
            // Se verifica el contador en lugar de usar Moq.Verify().
            Assert.That(verifiableContext.DisposeCallCount, Is.EqualTo(1));
        }
    }
}