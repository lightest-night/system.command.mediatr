using System.Linq;
using System.Reflection;
using LightestNight.System.ServiceResolution;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace LightestNight.System.Command.MediatR
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddCommandValidation(this IServiceCollection services, params Assembly[] validatorAssemblies)
        {
            if (validatorAssemblies.Any())
                services.AddCommandValidators(validatorAssemblies);
            
            return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }

        public static IServiceCollection AddCommandDecorators(this IServiceCollection services,
            params Assembly[] decoratorAssemblies)
        {
            AssemblyScanning.RegisterServices(services, decoratorAssemblies, new[]
            {
                new ConcreteRegistration
                {
                    InterfaceType = typeof(INotificationHandler<>),
                    AddIfAlreadyExists = true
                },
                new ConcreteRegistration
                {
                    InterfaceType = typeof(IRequestPreProcessor<>),
                    AddIfAlreadyExists = true
                },
                new ConcreteRegistration
                {
                    InterfaceType = typeof(IRequestPostProcessor<,>),
                    AddIfAlreadyExists = true
                },
                new ConcreteRegistration
                {
                    InterfaceType = typeof(IRequestExceptionHandler<,,>),
                    AddIfAlreadyExists = true
                },
                new ConcreteRegistration
                {
                    InterfaceType = typeof(IRequestExceptionAction<,>),
                    AddIfAlreadyExists = true
                }
            });

            return services;
        }
    }
}