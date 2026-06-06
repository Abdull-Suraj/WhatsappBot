using System.Net;
using System.Text.Json;
using BubbleShop.Domain.Exceptions;

namespace BubbleShop.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            EntityNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            DomainException => (HttpStatusCode.BadRequest, exception.Message),
            FluentValidation.ValidationException ve => (HttpStatusCode.UnprocessableEntity,
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            OperationCanceledException => (HttpStatusCode.ServiceUnavailable, "Request was cancelled."),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            error = message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
