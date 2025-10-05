using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using BienesRaicesAPI.Tools;

namespace BienesRaicesAPI.Tests.Tools
{
    // Controlador ficticio para la prueba, en un namespace que simula una versión.
    internal class DummyV1Controller { }
}

namespace BienesRaicesAPI.Tests.Tools
{
    [TestFixture]
    public class SwaggerGroupByVersionTests
    {
        [Test]
        public void Apply_WhenControllerHasVersionedNamespace_SetsApiExplorerGroupName()
        {
            // Arrange
            var convention = new SwaggerGroupByVersion();
            var controllerType = typeof(DummyV1Controller).GetTypeInfo();

            // Se crea un ControllerModel, que es el objeto que la convención modifica.
            var controllerModel = new ControllerModel(controllerType, new List<object>())
            {
                // ApiExplorer debe estar inicializado para que se le pueda asignar un GroupName.
                ApiExplorer = new ApiExplorerModel()
            };

            const string expectedGroupName = "tools";

            // Act
            convention.Apply(controllerModel);

            // Assert
            Assert.That(controllerModel.ApiExplorer.GroupName, Is.EqualTo(expectedGroupName));
        }
    }
}