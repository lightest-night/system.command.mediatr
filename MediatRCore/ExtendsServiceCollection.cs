using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LightestNight.System.Command.MediatR
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddCommandValidation(this IServiceCollection services)
            => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}