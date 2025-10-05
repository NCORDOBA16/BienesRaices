namespace Application.Tests.DTOs.VersionControls
{
    [TestFixture]
    public class VersionControlDto
    {
        [Test]
        public void VersionControlDto_Properties_Should_Be_Set_Correctly()
        {
            // Arrange
            var version = "1.0.0";

            // Act
            var dto = new Application.DTOs.VersionControls.VersionControlDto
            {
                VersionActual = version
            };

            // Assert
            Assert.That(dto.VersionActual, Is.EqualTo(version));
        }
    }
}
