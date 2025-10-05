namespace Application.Tests.Fixtures.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MyTestAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
    }
}
