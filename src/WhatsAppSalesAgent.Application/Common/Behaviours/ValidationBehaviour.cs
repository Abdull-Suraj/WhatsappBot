using FluentValidation;
using MediatR;
using WhatsAppSalesAgent.Application.Common.Models;

namespace WhatsAppSalesAgent.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

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

        var errors = failures.Select(f => f.ErrorMessage).ToList();

        // If TResponse is Result<T>, wrap failures
        var responseType = typeof(TResponse);
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(Result<>).MakeGenericType(responseType.GetGenericArguments()[0]);
            var failureMethod = resultType.GetMethod(nameof(Result<object>.Failure), new[] { typeof(IReadOnlyList<string>) });
            if (failureMethod is not null)
                return (TResponse)failureMethod.Invoke(null, new object[] { errors })!;
        }

        if (responseType == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(errors);
        }

        throw new FluentValidation.ValidationException(failures);
    }
}
