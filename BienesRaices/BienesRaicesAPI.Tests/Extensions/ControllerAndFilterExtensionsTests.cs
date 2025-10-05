using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using BienesRaicesAPI.Extensions;
using BienesRaicesAPI.Filters;

namespace BienesRaicesAPI.Tests.Extensions
{
    [TestFixture]
    public class ControllerAndFilterExtensionsTests
    {
        [Test]
        public void AddCustomControllersAndFilters_WhenCalled_ConfiguresControllersAndJsonOptionsCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCustomControllersAndFilters();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            var mvcOptions = serviceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
            var jsonOptions = serviceProvider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Multiple(() =>
            {
                // Verificar que el filtro ValidateModelAttribute se haya añadido.
                // Add<T> registra el filtro como un TypeFilterAttribute.
                Assert.That(mvcOptions.Filters.Any(f => f is TypeFilterAttribute attr && attr.ImplementationType == typeof(ValidateModelAttribute)), Is.True,
                    "El filtro ValidateModelAttribute no fue añadido a MvcOptions.");

                // Verificar que la convención SwaggerGroupByVersion se haya añadido.
                Assert.That(mvcOptions.Conventions.Any(), Is.True,
                    "La convención SwaggerGroupByVersion no fue añadida a MvcOptions.");

                // Verificar que la opción de serialización JSON se haya configurado correctamente.
                Assert.That(jsonOptions.JsonSerializerOptions.ReferenceHandler, Is.EqualTo(ReferenceHandler.IgnoreCycles),
                    "JsonSerializerOptions.ReferenceHandler no fue configurado como IgnoreCycles.");
            });
        }
    }
}