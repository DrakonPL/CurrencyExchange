using CurrencyExchange.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace CurrencyExchange.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
         where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count == 0)
                return await next();

            var message = string.Join("; ", failures.Select(f =>
                string.IsNullOrWhiteSpace(f.PropertyName)
                    ? f.ErrorMessage
                    : $"{f.PropertyName}: {f.ErrorMessage}"));

            // If any rule flagged "404", surface as not-found
            if (failures.Any(f => f.ErrorCode == "404"))
                throw new EntityNotFoundException(message);

            // Otherwise treat as bad request (400)
            throw new BadRequestException(message);
        }
    }
}
