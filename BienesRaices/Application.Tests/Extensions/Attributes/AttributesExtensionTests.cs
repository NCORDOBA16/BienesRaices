using Application.Extensions.Attributes;
using Application.Tests.Fixtures.Attributes;
using Application.Tests.Fixtures.Extensions;

namespace Application.Tests.Extensions.Attributes
{
    [TestFixture]
    public class AttributesExtensionTests
    {
        [Test]
        public void GetTypesDecoratedWith_ReturnsCorrectTypes()
        {
            // Act
            var types = AttributesExtension.GetTypesDecoratedWith<MyTestAttribute>();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(types, Is.Not.Empty);
                Assert.That(types, Has.Member(typeof(ClassWithMyTestAttribute)));
                Assert.That(types, Does.Not.Contain(typeof(ClassWithoutMyTestAttribute)));
            });
        }
    }
}
