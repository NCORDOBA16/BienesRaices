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

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In =ParameterLocation.Header,
                    Description = "Enter Bearer Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                        []
                    }
                });
            });

            return services;
        }
    }
}
