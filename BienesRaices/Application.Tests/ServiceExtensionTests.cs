using Application.Behaviours;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests
{
    [TestFixture]
    public class ServiceExtensionTests
    {
        private class DummyCommand : IRequest<Unit> { }

        [Test]
        public void AddApplicationServices_ShouldRegisterAllApplicationDependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();

            // Act
            services.AddApplicationServices();
            var serviceProvider = services.BuildServiceProvider();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(serviceProvider.GetService<IMapper>(), Is.Not.Null, "AutoMapper (IMapper) no fue registrado.");
                Assert.That(serviceProvider.GetService<IValidator<DummyCommand>>(), Is.Null, "Los validadores no deberían registrarse directamente si no existen en el ensamblado principal.");
            });

            Assert.Multiple(() =>
            {
                Assert.That(serviceProvider.GetService<IMediator>(), Is.Not.Null, "MediatR (IMediator) no fue registrado.");
                Assert.That(serviceProvider.GetService<ISender>(), Is.Not.Null, "MediatR (ISender) no fue registrado.");
            });

            var pipelineBehaviors = serviceProvider.GetServices<IPipelineBehavior<DummyCommand, Unit>>().ToList();

            Assert.That(pipelineBehaviors, Has.Count.EqualTo(2), "No se registró el número esperado de PipelineBehaviors.");
            Assert.Multiple(() =>
            {
                Assert.That(pipelineBehaviors.Any(b => b.GetType() == typeof(UnhandledExceptionBehaviour<DummyCommand, Unit>)), Is.True, "UnhandledExceptionBehaviour no fue registrado.");
                Assert.That(pipelineBehaviors.Any(b => b.GetType() == typeof(ValidationBehaviour<DummyCommand, Unit>)), Is.True, "ValidationBehaviour no fue registrado.");
            });
        }
    }
}