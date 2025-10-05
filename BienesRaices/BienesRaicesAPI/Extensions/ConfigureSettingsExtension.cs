using Application.Extensions.Environtments;
using Application.Statics.Configurations;
using Application.Statics.Externals;
using Application.Statics.FileServer;

namespace BienesRaicesAPI.Extensions
{
    public static class ConfigureSettingsExtension
    {
        public static void ConfigureApiSettings()
        {
            AddApiAuthSettings();
            AddDbSettings();
            AddExternalApiSettings();
            AddCloudinaryCredentials();
        }

        private static void AddApiAuthSettings()
        {

            ApiAuthSettings.ApiKey = EnvirontmentHelper.GetEnvironmentValue("API_KEY");
            ApiAuthSettings.CorsPolicyName = EnvirontmentHelper.GetEnvironmentValue("CORS_POLICY_NAME");
            ApiAuthSettings.Origin = EnvirontmentHelper.GetEnvironmentValue("CORS_ORIGIN");
        }

        private static void AddDbSettings()
        {
            DbSettings.DefaultConnection = EnvirontmentHelper.GetEnvironmentValue("DEFAULT_CONNECTION");
            DbSettings.TimeoutInMinutes = EnvirontmentHelper.GetIntValueFromEnvVariable("DB_TIMEOUT_IN_MINUTES");
        }


        private static void AddExternalApiSettings()
        {
            ExternalApiUrl.PokemonApiUrl = EnvirontmentHelper.GetEnvironmentValue("POKEMON_API_URL");
        }
        private static void AddCloudinaryCredentials()
        {
            CloudinaryCredentials.CloudName = EnvirontmentHelper.GetEnvironmentValue("CLOUDINARY_CLOUD_NAME");
            CloudinaryCredentials.ApiKey = EnvirontmentHelper.GetEnvironmentValue("CLOUDINARY_API_KEY");
            CloudinaryCredentials.ApiSecret = EnvirontmentHelper.GetEnvironmentValue("CLOUDINARY_API_SECRET");
        }
    }
}
