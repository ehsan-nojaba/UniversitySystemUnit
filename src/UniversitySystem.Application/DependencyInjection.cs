using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Behaviors;

namespace UniversitySystem.Application;

/// <summary>
/// Registers all Application layer services with the dependency injection container.
/// Called once from the Composition Root (UniversitySystem.Api).
///
/// Responsibilities:
///   - MediatR handlers (auto-discovered from this assembly)
///   - Pipeline behaviors in execution order
///   - FluentValidation validators (auto-discovered from this assembly)
///   - AutoMapper profiles (auto-discovered from this assembly)
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            // Behaviors execute in registration order (outermost → innermost):
            //   LoggingBehavior → PerformanceBehavior → ValidationBehavior → Handler
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddAutoMapper(cfg =>
        {
            // License key is read from the AUTOMAPPER_LICENSE_KEY environment variable.
            // See: https://automapper.io for community license details.
            cfg.AddMaps(assembly);
        });

        services.AddScoped<Common.Interfaces.IStudentCourseEligibilityService, Common.Services.StudentCourseEligibilityService>();
        services.AddScoped<Features.AdminPlanning.Services.IAdminPlanningService, Features.AdminPlanning.Services.AdminPlanningService>();
        services.AddScoped<Features.AdminPreRegistration.Services.IAdminPreRegistrationService, Features.AdminPreRegistration.Services.AdminPreRegistrationService>();
        services.AddScoped<Features.Auth.Services.IAuthService, Features.Auth.Services.AuthService>();
        services.AddScoped<Features.StudentPreRegistration.Services.IStudentPreRegistrationService, Features.StudentPreRegistration.Services.StudentPreRegistrationService>();

        return services;
    }
}
