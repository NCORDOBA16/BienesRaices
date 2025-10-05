using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using BienesRaicesAPI.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BienesRaicesAPI.Tests.Extensions
{
    [TestFixture]
    public class SwaggerExtensionsTests
    {
        [Test]
        public void AddSwagger_WhenCalled_ConfiguresSwaggerGenWithOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            services.AddSingleton(mockWebHostEnvironment.Object);


            // Act
            services.AddSwagger();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // Verificar que AddEndpointsApiExplorer fue llamado (registra IApiDescriptionGroupCollectionProvider)
            var apiExplorerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(Microsoft.AspNetCore.Mvc.ApiExplorer.IApiDescriptionGroupCollectionProvider));
            Assert.That(apiExplorerDescriptor, Is.Not.Null, "El servicio IApiDescriptionGroupCollectionProvider no fue registrado, indicando que AddEndpointsApiExplorer() no fue llamado.");

            // Verificar que SwaggerGen fue configurado correctamente
            var swaggerGeneratorOptions = serviceProvider.GetRequiredService<IOptions<SwaggerGeneratorOptions>>().Value;

            Assert.That(swaggerGeneratorOptions.SwaggerDocs, Contains.Key("v1"), "No se encontró el documento Swagger con la versión 'v1'.");

            var openApiInfo = swaggerGeneratorOptions.SwaggerDocs["v1"];
            Assert.Multiple(() =>
            {
                Assert.That(openApiInfo.Title, Is.EqualTo("BienesRaicesAPI"), "El título del documento Swagger no es 'BienesRaicesAPI'.");
                Assert.That(openApiInfo.Version, Is.EqualTo("1.0"), "La versión del documento Swagger no es '1.0'.");
            });
        }
    }
}