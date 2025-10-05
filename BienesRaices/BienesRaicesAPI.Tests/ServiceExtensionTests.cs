using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using BienesRaicesAPI;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BienesRaicesAPI.Tests
{
    [TestFixture]
    public class ServiceExtensionTests
    {
        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("API_KEY", "test-key");
            Environment.SetEnvironmentVariable("CORS_POLICY_NAME", "test-cors-policy");
            Environment.SetEnvironmentVariable("DEFAULT_CONNECTION", "Server=test;Database=testdb;User Id=testuser;Password=testpassword;");
            Environment.SetEnvironmentVariable("DB_TIMEOUT_IN_MINUTES", "5");
            Environment.SetEnvironmentVariable("POKEMON_API_URL", "https://pokeapi.co/api/v2/");
            Environment.SetEnvironmentVariable("CORS_ORIGIN", "https://example.com");
        }

        [TearDown]
        public void Teardown()
        {
            Environment.SetEnvironmentVariable("API_KEY", string.Empty);
            Environment.SetEnvironmentVariable("CORS_POLICY_NAME", string.Empty);
            Environment.SetEnvironmentVariable("DEFAULT_CONNECTION", string.Empty);
            Environment.SetEnvironmentVariable("DB_TIMEOUT_IN_MINUTES", string.Empty);
            Environment.SetEnvironmentVariable("POKEMON_API_URL", string.Empty);
            Environment.SetEnvironmentVariable("CORS_ORIGIN", string.Empty);
        }

        [Test]
        public void AddApiServices_WhenCalled_RegistersAllServices()
        {
            // Arrange
            var services = new ServiceCollection();
            // AddSwagger() tiene una dependencia interna de IWebHostEnvironment
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            services.AddSingleton(mockWebHostEnvironment.Object);
            services.AddLogging();

            // Act
            services.AddApiServices();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            Assert.Multiple(() =>
            {
                // Verificar AddCustomControllersAndFilters()
                var controllerFactory = services.FirstOrDefault(d => d.ServiceType == typeof(IControllerFactory));
                Assert.That(controllerFactory, Is.Not.Null, "IControllerFactory no fue registrado.");
                Assert.That(controllerFactory!.ImplementationType?.Name, Is.EqualTo("DefaultControllerFactory"), "El tipo de implementación para IControllerFactory no es DefaultControllerFactory.");

                // Verificar AddEndpointsApiExplorer()
                var apiExplorer = services.FirstOrDefault(d => d.ServiceType == typeof(IApiDescriptionGroupCollectionProvider));
                Assert.That(apiExplorer, Is.Not.Null, "IApiDescriptionGroupCollectionProvider no fue registrado por AddEndpointsApiExplorer().");

                // Verificar AddSwagger()
                var swaggerOptions = serviceProvider.GetService<IOptions<SwaggerGeneratorOptions>>();
                Assert.That(swaggerOptions, Is.Not.Null, "Las opciones de Swagger (SwaggerGeneratorOptions) no fueron registradas.");

                // Verificar AddCors()
                var corsService = serviceProvider.GetService<ICorsService>();
                Assert.That(corsService, Is.Not.Null, "ICorsService no fue registrado por AddCors().");
            });
        }
    }
}