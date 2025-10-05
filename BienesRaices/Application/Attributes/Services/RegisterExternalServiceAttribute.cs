using Microsoft.Extensions.DependencyInjection;

namespace Application.Attributes.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterExternalServiceAttribute(ServiceLifetime lifeTime) : Attribute
    {
        public ServiceLifetime LifeTime { get; set; } = lifeTime;

        public const string BaseAddress = "/";
    }
}
