using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Exceptions;
using MediatR;

namespace LightestNight.System.Command.MediatR
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<ICommandValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<ICommandValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = await Task.WhenAll(_validators.Select(validator => validator.Validate(command, cancellationToken)));
            if (!validationResult.Any(error => error.Any()))
                return await next();

            throw new DomainException("An error occurred processing a domain operation",
                validationResult.SelectMany(error => error).ToArray());
        }
    }
}