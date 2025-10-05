using Application.Statics.Externals;

namespace Application.Tests.Statics.Externals
{
    [TestFixture]
    public class ExternalApiUrlTests
    {
        [SetUp]
        public void SetUp()
        {
            ExternalApiUrl.PokemonApiUrl = "https://pokeapi.co/api/v2/";
        }

        [TearDown]
        public void TearDown()
        {
            ExternalApiUrl.PokemonApiUrl = string.Empty;
        }

        [Test]
        public void PokemonApiUrl_WhenSetAndGet_ShouldReturnCorrectValue()
        {
            // Arrange & Act
            var actualPokemonApiUrl = ExternalApiUrl.PokemonApiUrl;

            // Assert
            Assert.That(actualPokemonApiUrl, Is.EqualTo("https://pokeapi.co/api/v2/"));
        }
    }
}
