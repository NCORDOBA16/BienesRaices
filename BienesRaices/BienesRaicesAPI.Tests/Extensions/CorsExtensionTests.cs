using Application.Statics.Configurations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using BienesRaicesAPI.Extensions;

namespace BienesRaicesAPI.Tests.Extensions
{
    [TestFixture]
    public class CorsExtensionTests
    {
        private const string TestPolicyName = "TestCorsPolicy";

        [SetUp]
        public void SetUp()
        {
            ApiAuthSettings.CorsPolicyName = TestPolicyName;
            ApiAuthSettings.Origin = "https://example.com";
        }

        [TearDown]
        public void TearDown()
        {
            // Restablecer el valor para no afectar otras pruebas
            ApiAuthSettings.CorsPolicyName = string.Empty;
            ApiAuthSettings.Origin = string.Empty;
        }

        [Test]
        public void AddCustomCors_WithConfiguredOrigins_ShouldApplySpecificOrigins()
        {
            // Arrange
            var services = new ServiceCollection();
            var allowedOrigins = new[] { "https://example.com" };

            // Act
            services.AddCustomCors();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;
            var policy = corsOptions.GetPolicy(TestPolicyName);

            Assert.That(policy, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(policy.AllowAnyOrigin, Is.False, "La política no debería permitir cualquier origen.");
                Assert.That(policy.Origins, Is.EquivalentTo(allowedOrigins), "Los orígenes no coinciden con la configuración.");
                Assert.That(policy.AllowAnyMethod, Is.True, "La política debería permitir cualquier método.");
                Assert.That(policy.AllowAnyHeader, Is.True, "La política debería permitir cualquier cabecera.");
                Assert.That(policy.ExposedHeaders, Does.Contain("content-disposition"), "La política debería exponer 'content-disposition'.");
            });
        }

        [Test]
        public void AddCustomCors_WithoutConfiguredOrigins_ShouldApplyDefaultRestrictivePolicy()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCustomCors();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;
            var policy = corsOptions.GetPolicy(TestPolicyName);

            Assert.That(policy, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(policy.AllowAnyOrigin, Is.False, "La política no debería permitir cualquier origen.");
                Assert.That(policy.Origins, Does.Contain("https://example.com"), "La política por defecto debería contener 'https://example.com'.");
                Assert.That(policy.Origins, Has.Count.EqualTo(1), "La política por defecto solo debería tener un origen.");
            });
        }
    }
}