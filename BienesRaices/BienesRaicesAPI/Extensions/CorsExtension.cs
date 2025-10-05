using Application.Statics.Configurations;

namespace BienesRaicesAPI.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ApiAuthSettings.CorsPolicyName, builder =>
                {
                    builder.WithOrigins(ApiAuthSettings.Origin)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    builder.WithExposedHeaders("content-disposition");
                    builder.SetIsOriginAllowed(origin => true);
                });
            });
            return services;
        }
    }
}
