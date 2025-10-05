using Application.Tests.Fixtures.Attributes;

namespace Application.Tests.Fixtures.Extensions
{
    [MyTest]
    public class ClassWithMyTestAttribute
    {
        public string Description { get; set; } = string.Empty;
    }
}
