using Swashbuckle.AspNetCore.Annotations;
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
        services.AddControllers(options => options.Conventions.Add(new ApiResponseConvention()));
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:5173", "http://localhost:3000"];
        services.AddCors(options => options.AddPolicy("Ui", policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
        // ── 2. Global Exception Handling & ProblemDetails ──────────────────────
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        // ── 3. Health Checks ───────────────────────────────────────────────────
        services.AddHealthChecks();
        // تولید سند OpenAPI با امکانات داخلی ASP.NET Core؛ نمایش توسط Scalar.
        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo { Title = "University System API", Version = "v1", Description = "سامانه پیش‌انتخاب واحد و برنامه‌ریزی ترم دانشگاه" };
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", Description = "توکن دریافتی از ورود را وارد کنید." };
                foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations?.Values.AsEnumerable() ?? []))
                {
                    if (operation.Security?.Count > 0)
                    {
                        operation.Security = [new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] }];
                    }
                }
                return Task.CompletedTask;
            });
            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                var metadata = context.Description.ActionDescriptor.EndpointMetadata;
                var description = metadata.OfType<SwaggerOperationAttribute>().FirstOrDefault();
                if (description is not null)
                {
                    operation.Summary = description.Summary;
                    operation.Description = description.Description;
                }
                if (metadata.OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>().Any() && !metadata.OfType<Microsoft.AspNetCore.Authorization.IAllowAnonymous>().Any())
                {
                    operation.Security = [new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("Bearer")] = [] }];
                }
                return Task.CompletedTask;
            });
        });
        return services;
    }
}
