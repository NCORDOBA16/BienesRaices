using Microsoft.Extensions.DependencyInjection;

namespace Application.Attributes.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterServiceAttribute(ServiceLifetime lifeTime) : Attribute
    {
        public ServiceLifetime LifeTime { get; set; } = lifeTime;
    }
}
