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
    }
}