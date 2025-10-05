using System.Text.Json.Serialization;
using BienesRaicesAPI.Filters;
using BienesRaicesAPI.Tools;

namespace BienesRaicesAPI.Extensions
{
    public static class ControllerAndFilterExtensions
    {
        public static IServiceCollection AddCustomControllersAndFilters(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
                options.Conventions.Add(new SwaggerGroupByVersion());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            return services;
        }
    }
}
