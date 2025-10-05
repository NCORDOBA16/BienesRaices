using Application.Exceptions;
using Application.Extensions.Environtments;

namespace Application.Tests.Extensions.Environtments
{
    [TestFixture]
    public class EnvirontmentHelperTests
    {
        private const string TestVariable = "UNIT_TEST_VARIABLE";

        [TearDown]
        public void TearDown()
        {
            // Limpia la variable de entorno después de cada prueba para garantizar el aislamiento.
            Environment.SetEnvironmentVariable(TestVariable, null);
        }

        #region GetEnvironmentValue Tests

        [Test]
        public void GetEnvironmentValue_WhenVariableExists_ShouldReturnValue()
        {
            // Arrange
            var expectedValue = "test_value";
            Environment.SetEnvironmentVariable(TestVariable, expectedValue);

            // Act
            var result = EnvirontmentHelper.GetEnvironmentValue(TestVariable);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetEnvironmentValue_WhenVariableDoesNotExist_ShouldThrowApiException()
        {
            // Arrange
            var expectedMessage = $"Variable {TestVariable} no existe";

            // Act & Assert
            var ex = Assert.Throws<ApiException>(() => EnvirontmentHelper.GetEnvironmentValue(TestVariable));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        #endregion

        #region GetIntValueFromEnvVariable Tests

        [Test]
        public void GetIntValueFromEnvVariable_WhenVariableIsValidInteger_ShouldReturnIntValue()
        {
            // Arrange
            var expectedValue = 123;
            Environment.SetEnvironmentVariable(TestVariable, expectedValue.ToString());

            // Act
            var result = EnvirontmentHelper.GetIntValueFromEnvVariable(TestVariable);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetIntValueFromEnvVariable_WhenVariableIsNotAnInteger_ShouldThrowFormatException()
        {
            // Arrange
            Environment.SetEnvironmentVariable(TestVariable, "not-an-integer");

            // Act & Assert
            Assert.Throws<FormatException>(() => EnvirontmentHelper.GetIntValueFromEnvVariable(TestVariable));
        }

        [Test]
        public void GetIntValueFromEnvVariable_WhenVariableDoesNotExist_ShouldThrowApiException()
        {
            // Arrange
            var expectedMessage = $"Variable {TestVariable} no existe";

            // Act & Assert
            // Esta prueba confirma que la ApiException de GetEnvironmentValue se propaga.
            var ex = Assert.Throws<ApiException>(() => EnvirontmentHelper.GetIntValueFromEnvVariable(TestVariable));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        #endregion
    }
}