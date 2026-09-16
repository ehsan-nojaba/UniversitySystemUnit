using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UniversitySystem.Application.Common.Behaviors;

/// <summary>
/// مدت اجرای درخواست Application را اندازه می‌گیرد و درخواست کند را در لاگ مشخص می‌کند.
/// </summary>
public sealed class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Requests that take longer than this threshold are considered slow and trigger a warning log.
    /// </summary>
    private const int SlowRequestThresholdMilliseconds = 500;

    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdMilliseconds)
        {
            _logger.LogWarning(
                "Slow request detected: {RequestName} took {ElapsedMilliseconds}ms (threshold: {ThresholdMs}ms)",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                SlowRequestThresholdMilliseconds);
        }

        return response;
    }
}
