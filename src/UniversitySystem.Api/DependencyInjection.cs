using Microsoft.OpenApi;
using UniversitySystem.Api.Infrastructure;

namespace UniversitySystem.Api;
/// <summary>
/// سرویس‌های Api را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ── 1. Controllers & API Conventions ───────────────────────────────────
        services.AddControllers();
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:5173", "http://localhost:3000"];
        services.AddCors(options => options.AddPolicy("Ui", policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
        // ── 2. Global Exception Handling & ProblemDetails ──────────────────────
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        // ── 3. Health Checks ───────────────────────────────────────────────────
        services.AddHealthChecks();
        // ── 4. OpenAPI / Swagger Documentation with JWT Bearer ─────────────────
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "University System API", Version = "v1", Description = "University Management System Web API built with .NET 10 & Clean Architecture." });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT Bearer token format: Bearer {your token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };
            options.AddSecurityDefinition("Bearer", securityScheme);
            var securitySchemeRef = new OpenApiSecuritySchemeReference("Bearer");
            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement { { securitySchemeRef, new List<string>() } });
        });
        return services;
    }
}
