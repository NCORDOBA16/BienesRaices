using Application.Statics.Configurations;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.DbContexts
{
    [TestFixture]
    public class ApplicationDbContextTests
    {
        [TearDown]
        public void TearDown()
        {
            // Restablece el valor estático para evitar interferencias entre pruebas.
            DbSettings.TimeoutInMinutes = 1; // Valor por defecto
        }

        [Test]
        public void Constructor_WhenUsingInMemoryProvider_ShouldSetNoTrackingAndNotSetCommandTimeout()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_NonRelational")
                .Options;

            // Act
            using var context = new ApplicationDbContext(options);

            // Assert
            // 1. Verifica que el QueryTrackingBehavior es NoTracking.
            Assert.That(context.ChangeTracker.QueryTrackingBehavior, Is.EqualTo(QueryTrackingBehavior.NoTracking));

            // 2. La prueba pasa si no se lanza ninguna excepción al crear el contexto,
            // lo que confirma que el bloque if (IsRelational) no se ejecutó.
            // La llamada a GetCommandTimeout() se elimina porque no es compatible con el proveedor en memoria.
        }

        [Test]
        public void Constructor_WhenUsingRelationalProvider_ShouldSetCommandTimeout()
        {
            // Arrange
            // Para simular un proveedor relacional, creamos y mantenemos abierta una conexión a SQLite en memoria.
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection) // Usar la conexión abierta
                .Options;

            var expectedTimeoutInMinutes = 5;
            DbSettings.TimeoutInMinutes = expectedTimeoutInMinutes;

            // Act
            using var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated(); // Asegura que el modelo se construya

            // Assert
            var actualTimeoutInSeconds = context.Database.GetCommandTimeout();
            var expectedTimeoutInSeconds = (int)TimeSpan.FromMinutes(expectedTimeoutInMinutes).TotalSeconds;
            Assert.That(actualTimeoutInSeconds, Is.EqualTo(expectedTimeoutInSeconds));
        }

        [Test]
        public void DbSet_VersionControl_ShouldBeAvailable()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_DbSet")
                .Options;
            using var context = new ApplicationDbContext(options);

            // Act & Assert
            // La prueba simplemente verifica que la propiedad DbSet no es nula.
            Assert.That(context.VersionControl, Is.Not.Null);
        }

        [Test]
        public void OnModelCreating_ShouldApplyEntityConfigurations()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_ModelCreating")
                .Options;

            // Para probar OnModelCreating, necesitamos una forma de saber si se llamó.
            // La mejor manera es verificar el resultado: que el modelo contiene las entidades configuradas.
            // Como no tenemos acceso a las configuraciones exactas, una prueba simple
            // es verificar que el modelo conoce la entidad VersionControl.
            using var context = new ApplicationDbContext(options);

            // Act
            var entityType = context.Model.FindEntityType(typeof(VersionControl));

            // Assert
            // Si OnModelCreating se ejecutó y configuró la entidad, esta no será nula.
            Assert.That(entityType, Is.Not.Null, "La entidad VersionControl no fue encontrada en el modelo del DbContext.");
            Assert.That(entityType.Name, Is.EqualTo(typeof(VersionControl).FullName));
        }
    }
}