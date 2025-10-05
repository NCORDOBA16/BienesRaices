using Application.Statics.Configurations;
using Application.Statics.Externals;
using BienesRaicesAPI.Extensions;

namespace BienesRaicesAPI.Tests.Extensions
{
    [TestFixture]
    public class ConfigureSettingsExtensionTests
    {
        private readonly Dictionary<string, string?> _environmentVariables = new()
        {
            { "API_KEY", "test_api_key" },
            { "CORS_POLICY_NAME", "test_cors_policy" },
            { "DEFAULT_CONNECTION", "test_db_connection_string" },
            { "DB_TIMEOUT_IN_MINUTES", "5" },
            { "POKEMON_API_URL", "https://pokeapi.co/api/v2/" },
            { "CORS_ORIGIN", "http://localhost" }
        };

        [SetUp]
        public void SetUp()
        {
            foreach (var kvp in _environmentVariables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var kvp in _environmentVariables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, null);
            }
        }

        [Test]
        public void ConfigureApiSettings_WhenCalled_SetsAllSettingsFromEnvironmentVariables()
        {
            // Arrange
            // Las variables de entorno se establecen en el método SetUp.

            // Act
            ConfigureSettingsExtension.ConfigureApiSettings();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(ApiAuthSettings.ApiKey, Is.EqualTo(_environmentVariables["API_KEY"]));
                Assert.That(ApiAuthSettings.CorsPolicyName, Is.EqualTo(_environmentVariables["CORS_POLICY_NAME"]));
                Assert.That(DbSettings.DefaultConnection, Is.EqualTo(_environmentVariables["DEFAULT_CONNECTION"]));
                Assert.That(DbSettings.TimeoutInMinutes, Is.EqualTo(int.Parse(_environmentVariables["DB_TIMEOUT_IN_MINUTES"]!)));
                Assert.That(ExternalApiUrl.PokemonApiUrl, Is.EqualTo(_environmentVariables["POKEMON_API_URL"]));
                Assert.That(ApiAuthSettings.Origin, Is.EqualTo(_environmentVariables["CORS_ORIGIN"]));
            });
        }
    }
}