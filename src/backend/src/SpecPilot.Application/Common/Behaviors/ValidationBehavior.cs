using FluentValidation;
using MediatR;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken))))
            .SelectMany(x => x.Errors)
            .Where(x => x is not null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var errorMessage = string.Join(" ", failures.Select(x => x.ErrorMessage).Distinct());
        var error = Error.Validation("common.validation_error", errorMessage);

        return CreateFailureResponse(error);
    }

    private static TResponse CreateFailureResponse(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = typeof(TResponse).GetGenericArguments()[0];
            var failureMethod = typeof(Result)
                .GetMethods()
                .Single(x => x.Name == nameof(Result.Failure) && x.IsGenericMethodDefinition);

            var genericFailureMethod = failureMethod.MakeGenericMethod(valueType);
            return (TResponse)genericFailureMethod.Invoke(null, [error])!;
        }

        throw new InvalidOperationException(
            $"O tipo de resposta '{typeof(TResponse).Name}' nao suporta falhas de validacao baseadas em Result.");
    }
}
