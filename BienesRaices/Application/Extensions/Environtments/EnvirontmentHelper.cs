using Application.Exceptions;

namespace Application.Extensions.Environtments
{
    public static class EnvirontmentHelper
    {
        public static string GetEnvironmentValue(string variable)
        {
            return Environment.GetEnvironmentVariable(variable) ?? throw new ApiException($"Variable {variable} no existe");
        }

        public static int GetIntValueFromEnvVariable(string variable)
        {
            var stringValue = GetEnvironmentValue(variable) ?? "0";
            return int.Parse(stringValue);
        }
    }
}
