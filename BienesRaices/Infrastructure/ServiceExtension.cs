using System.Reflection;
using Application.Attributes.Services;
using Application.Contracts.Persistence.Common.BaseRepository;
using Application.Contracts.Services.ExternalApiServices;
using Application.Extensions.Attributes;
using Application.Statics.Configurations;
using Infrastructure.DbContexts;
using Infrastructure.Repositories.Common.BaseRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceExtension
    {
        private const string BaseApiServiceName = nameof(IBaseApiService);
        private const string DisposableInterfaceName = nameof(IDisposable);
        private const string BaseAddress = "https://www.google.com";
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            ConfigureDatabase(services);
            ConfigureHttpContextAccessor(services);
            RegisterRepositories(services);
            RegisterExternalApiServices(services);
        }

        private static void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(DbSettings.DefaultConnection)
            );
        }

        private static void ConfigureHttpContextAccessor(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.RegisterServicesDecoratedWithRegisterServiceAttribute();
        }

        private static void RegisterExternalApiServices(this IServiceCollection services, IEnumerable<Type>? typesToRegister = null)
        {
            var types = typesToRegister ?? AttributesExtension.GetTypesDecoratedWith<RegisterExternalServiceAttribute>();

            foreach (var type in types)
            {
                RegisterExternalService(services, type);
            }
        }

        private static void RegisterExternalService(IServiceCollection services, Type type)
        {
            var attribute = type.GetCustomAttribute<RegisterExternalServiceAttribute>();
            var interfaceType = type.GetInterfaces().First(x => x.Name != BaseApiServiceName && x.Name != DisposableInterfaceName);

            // Primero registrar el tipo concreto
            services.Add(new ServiceDescriptor(type, type, attribute!.LifeTime));

            // Luego el cliente tipado que usa la interfaz como clave
            services.AddHttpClient(type.Name)
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(BaseAddress))
                .AddTypedClient((cl, sp) => sp.GetRequiredService(interfaceType));

            // Finalmente registrar la interfaz que delega al tipo concreto
            services.Add(new ServiceDescriptor(interfaceType, sp => sp.GetRequiredService(type), attribute.LifeTime));
        }

        private static void RegisterServicesDecoratedWithRegisterServiceAttribute(this IServiceCollection services, IEnumerable<Type>? typesToRegister = null)
        {
            var types = typesToRegister ?? AttributesExtension.GetTypesDecoratedWith<RegisterServiceAttribute>();

            foreach (var type in types)
            {
                RegisterService(services, type);
            }
        }

        private static void RegisterService(IServiceCollection services, Type type)
        {
            var attribute = type.GetCustomAttribute<RegisterServiceAttribute>();
            var interfaceType = type.GetInterfaces().FirstOrDefault()!;
            services.Add(new ServiceDescriptor(interfaceType, type, attribute!.LifeTime));
        }
    }
}
