using MediatR;
using Microsoft.Extensions.Logging;
using WhatsAppSalesAgent.Application.Common.Models;
using WhatsAppSalesAgent.Domain.Exceptions;

namespace WhatsAppSalesAgent.Application.Common.Behaviours;

public class ExceptionHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehaviour(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain exception in {RequestName}: {Message}", typeof(TRequest).Name, ex.Message);
            return CreateFailureResult(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }

    private static TResponse CreateFailureResult(string error)
    {
        var responseType = typeof(TResponse);

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(Result<>).MakeGenericType(responseType.GetGenericArguments()[0]);
            var failureMethod = resultType.GetMethod(nameof(Result<object>.Failure), new[] { typeof(string) });
            if (failureMethod is not null)
                return (TResponse)failureMethod.Invoke(null, new object[] { error })!;
        }

        if (responseType == typeof(Result))
            return (TResponse)(object)Result.Failure(error);

        throw new InvalidOperationException($"Cannot create failure result for type {responseType.Name}");
    }
}
