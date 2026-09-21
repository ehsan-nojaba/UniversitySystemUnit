using FluentValidation;
using System.Reflection;
using UniversitySystem.Application.Features.AdminReports.Services;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;
using UniversitySystem.Application.Features.StudentResults.Services;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Behaviors;

namespace UniversitySystem.Application;
/// <summary>
/// سرویس‌های Application را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است.
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
            cfg.AddOpenBehavior(typeof(LoggingBehavior<, >));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<, >));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<, >));
        });
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<Common.Interfaces.IStudentCourseEligibilityService, Common.Services.StudentCourseEligibilityService>();
        services.AddScoped<Features.AdminPlanning.Services.IAdminPlanningService, Features.AdminPlanning.Services.AdminPlanningService>();
        services.AddScoped<Features.AdminPreRegistration.Services.IAdminPreRegistrationService, Features.AdminPreRegistration.Services.AdminPreRegistrationService>();
        services.AddScoped<Features.Auth.Services.IAuthService, Features.Auth.Services.AuthService>();
        services.AddScoped<Features.StudentPreRegistration.Services.IStudentPreRegistrationService, Features.StudentPreRegistration.Services.StudentPreRegistrationService>();
        services.AddScoped<Features.CourseOfferings.Services.ICourseOfferingService, Features.CourseOfferings.Services.CourseOfferingService>();
        services.AddScoped<Features.CourseOfferings.Services.IProfessorScheduleConflictChecker, Features.CourseOfferings.Services.ProfessorScheduleConflictChecker>();
        services.AddScoped<Features.TeachingAssignments.Services.ITeachingAssignmentService, Features.TeachingAssignments.Services.TeachingAssignmentService>();
        services.AddScoped<Features.Enrollments.Services.EnrollmentService>();
        services.AddScoped<Features.StudentResults.Services.StudentResultService>();
        services.AddScoped<Features.AdminReports.Services.AdminReportService>();
        services.AddScoped<Features.ProfessorTeachingRequests.Services.ProfessorTeachingRequestService>();
        services.AddScoped<Features.AcademicWorkflow.AcademicWorkflowService>();
        services.AddScoped<Features.MajorCurricula.MajorCurriculumService>();
        services.AddScoped<Features.UiSupport.Services.UiSupportService>();
        return services;
    }
}
