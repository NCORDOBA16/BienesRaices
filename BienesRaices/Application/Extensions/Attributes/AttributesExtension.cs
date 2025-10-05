namespace Application.Extensions.Attributes
{
    public static class AttributesExtension
    {
        public static IEnumerable<Type> GetTypesDecoratedWith<T>() where T : Attribute
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName!.StartsWith("Application") || x.FullName.StartsWith("Infrastructure"));

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(T), false).Length > 0)
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}
