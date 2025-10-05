using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using BienesRaicesAPI.Filters;


namespace BienesRaicesAPI.Tests.Filters
{
    [TestFixture]
    public class ValidateModelAttributeTests
    {
        private ValidateModelAttribute _validateModelAttribute;

        [SetUp]
        public void SetUp()
        {
            _validateModelAttribute = new ValidateModelAttribute();
        }

        [Test]
        public void OnResultExecuting_ModelStateIsInvalid_SetsBadRequestResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new Mock<ActionDescriptor>().Object;
            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor,
            };

            var context = new ResultExecutingContext(
                actionContext,
                [],
                new Mock<IActionResult>().Object,
                new Mock<Controller>().Object
            );

            context.ModelState.AddModelError("request", "error message request");
            context.ModelState.AddModelError("$.key", "error message");

            // Act
            _validateModelAttribute.OnResultExecuting(context);

            // Assert
            Assert.That(context.Result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = context.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            var apiResponse = badRequestResult!.Value as WrapperResponse<object>;
            Assert.That(apiResponse, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(apiResponse!.Succeeded, Is.False);
                Assert.That(apiResponse.Message, Is.EqualTo("Error en la petición"));
                Assert.That(apiResponse.Errors, Has.Count.EqualTo(1));
                Assert.That(apiResponse!.Errors.First(), Is.EqualTo("Algunos valores no coinciden con los tipos de datos del servicio solicitado: [key]"));
            });
        }
    }
}

