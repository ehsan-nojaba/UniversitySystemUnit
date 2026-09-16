using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Common.Exceptions;

namespace UniversitySystem.Api.Infrastructure;

/// <summary>
/// خطاهای اعتبارسنجی، دسترسی و قواعد کسب‌وکار را به پاسخ HTTP استاندارد ProblemDetails تبدیل می‌کند.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, title, detail, extensions) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (extensions != null)
        {
            foreach (var (key, value) in extensions)
            {
                problemDetails.Extensions[key] = value;
            }
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, string Title, string Detail, IDictionary<string, object?>? Extensions) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation errors occurred.",
                new Dictionary<string, object?> { ["errors"] = validationException.Errors }
            ),

            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFoundException.Message,
                null
            ),

            ForbiddenAccessException forbiddenAccessException => (
                StatusCodes.Status403Forbidden,
                "Forbidden",
                forbiddenAccessException.Message,
                null
            ),

            UnauthorizedAccessException unauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                unauthorizedAccessException.Message,
                null
            ),

            BusinessException businessException => (
                StatusCodes.Status400BadRequest,
                "Business Rule Violation",
                businessException.Message,
                null
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.",
                null
            )
        };
    }
}
