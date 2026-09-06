using Microsoft.Extensions.DependencyInjection;

namespace UniversitySystem.Infrastructure;

/// <summary>
/// Registers Infrastructure layer services with the dependency injection container.
/// Called from the Composition Root (UniversitySystem.Api).
/// Covers cross-cutting concerns: authentication, external services, etc.
/// </summary>
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Authentication, external HTTP clients, and cross-cutting services
        // will be registered here in future tasks.
        return services;
    }
}
