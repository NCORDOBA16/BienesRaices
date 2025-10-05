namespace Application.Extensions.Values
{
    public static class ValuesHelperExtension
    {
        public static List<T> ValueToOneItemList<T>(this T value)
        {
            return [value];
        }

        public static string ThrowIfNullOrEmpty(this string? value, string? message = "Argument is not valid")
        {
            if (string.IsNullOrEmpty(value?.Trim() ?? string.Empty))
            {
                throw new ArgumentException(message);
            }

            return value!;
        }
    }
}
