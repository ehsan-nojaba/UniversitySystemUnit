using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Common.Exceptions;

namespace UniversitySystem.Api.Infrastructure;

/// <summary>
/// خطاهای ورودی، دسترسی و قواعد کسب‌وکار را به پاسخ HTTP استاندارد ProblemDetails تبدیل می‌کند.
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
        if (exception is OperationCanceledException)
        {
            _logger.LogDebug("Request was cancelled: {Path}", httpContext.Request.Path);
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = 499;
            }

            return true;
        }

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
            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "اطلاعات پیدا نشد",
                notFoundException.Message,
                null
            ),

            ForbiddenAccessException forbiddenAccessException => (
                StatusCodes.Status403Forbidden,
                "دسترسی غیرمجاز",
                forbiddenAccessException.Message,
                null
            ),

            PreRegistrationLimitException preRegistrationLimitException => (
                StatusCodes.Status409Conflict,
                "سقف پیش‌ثبت‌نام تکمیل شده است",
                preRegistrationLimitException.Message,
                null
            ),

            UnauthorizedAccessException unauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "احراز هویت ناموفق بود",
                unauthorizedAccessException.Message,
                null
            ),

            BusinessException businessException => (
                StatusCodes.Status400BadRequest,
                "عملیات مجاز نیست",
                businessException.Message,
                null
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                "خطای داخلی سامانه",
                "خطای پیش‌بینی‌نشده‌ای رخ داد. لطفاً دوباره تلاش کنید.",
                null
            )
        };
    }
}
