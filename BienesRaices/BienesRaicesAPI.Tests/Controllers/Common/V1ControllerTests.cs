using Microsoft.AspNetCore.Mvc;
using BienesRaicesAPI.Controllers.Common;

namespace BienesRaicesAPI.Tests.Controllers.Common
{
    [TestFixture]
    public class V1ControllerTests
    {
        [Test]
        public void HasCorrectRouteAttribute()
        {
            var type = typeof(V1Controller);
            var attribute = type.GetCustomAttributes(typeof(RouteAttribute), true).FirstOrDefault() as RouteAttribute;

            Assert.That(attribute, Is.Not.Null);
            Assert.That(attribute.Template, Is.EqualTo("api/v1/[controller]"));
        }
    }
}
