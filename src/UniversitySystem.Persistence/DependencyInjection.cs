using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence;

/// <summary>
/// Registers all Persistence layer services with the dependency injection container.
/// Called once from the Composition Root (UniversitySystem.Api).
///
/// Responsibilities:
///   - EF Core DbContext registration with SQL Server provider
///   - IApplicationDbContext binding to ApplicationDbContext implementation
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured. " +
                "Set it in appsettings.json or via environment variable " +
                "ConnectionStrings__DefaultConnection.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    // Migrations are co-located with the DbContext in this assembly.
                    sqlServerOptions.MigrationsAssembly(
                        typeof(ApplicationDbContext).Assembly.FullName);
                }));

        // Bind the Application contract to the concrete implementation.
        // Handlers use IApplicationDbContext, never ApplicationDbContext directly.
        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork, Repositories.UnitOfWork>();
        services.AddScoped<Application.Features.AdminPlanning.Repositories.IAdminPlanningRepository, Repositories.AdminPlanningRepository>();
        services.AddScoped<Application.Features.AdminPreRegistration.Repositories.IAdminPreRegistrationRepository, Repositories.AdminPreRegistrationRepository>();
        services.AddScoped<Application.Features.Auth.Repositories.IAuthRepository, Repositories.AuthRepository>();
        services.AddScoped<Application.Features.StudentPreRegistration.Repositories.IStudentPreRegistrationRepository, Repositories.StudentPreRegistrationRepository>();
        services.AddScoped<Application.Features.CourseOfferings.Repositories.ICourseOfferingRepository, Repositories.CourseOfferingRepository>();
        services.AddScoped<Application.Features.TeachingAssignments.Repositories.ITeachingAssignmentRepository, Repositories.TeachingAssignmentRepository>();

        return services;
    }
}
