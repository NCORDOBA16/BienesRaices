using BienesRaicesAPI.Extensions;

namespace BienesRaicesAPI
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            ConfigureSettingsExtension.ConfigureApiSettings();

            services.AddCustomControllersAndFilters();

            services.AddEndpointsApiExplorer();

            services.AddSwagger();

            services.AddCustomCors();

            return services;
        }
    }
}