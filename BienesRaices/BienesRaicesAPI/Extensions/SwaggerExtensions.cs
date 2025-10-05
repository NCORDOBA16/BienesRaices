using Microsoft.OpenApi.Models;

namespace BienesRaicesAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BienesRaicesAPI", Version = "1.0" });
            });

            return services;
        }
    }
}
