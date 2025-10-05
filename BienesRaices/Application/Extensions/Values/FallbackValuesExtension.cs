namespace Application.Extensions.Values
{
    public static class FallbackValuesExtension
    {
        public static string? FallbackValue(this string? value, string? fallbackValue)
        {
            return string.IsNullOrEmpty(value) ? fallbackValue : value;
        }
    }
}
