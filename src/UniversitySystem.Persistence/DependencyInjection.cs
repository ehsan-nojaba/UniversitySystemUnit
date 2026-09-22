using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.AdminReports.Repositories;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence;
/// <summary>
/// سرویس‌های Persistence را در DI ثبت می‌کند؛ نقطه اتصال قراردادها و پیاده‌سازی‌های این لایه است.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("رشته اتصال پیش‌فرض تنظیم نشده است. آن را در appsettings.json یا متغیر محیطی ConnectionStrings__DefaultConnection قرار دهید.");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptions =>
        {
            // Migrations are co-located with the DbContext in this assembly.
            sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        }));
        // قرارداد داخلی دیتابیس؛ Handlerها از سرویس‌های Application و ریپازیتوری‌ها استفاده می‌کنند.
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork, Repositories.UnitOfWork>();
        services.AddScoped<IStudentEligibilityRepository, Repositories.StudentEligibilityRepository>();
        services.AddScoped<Application.Features.CourseOfferings.Repositories.IProfessorScheduleRepository, Repositories.ProfessorScheduleRepository>();
        services.AddScoped<Application.Features.AdminPlanning.Repositories.IAdminPlanningRepository, Repositories.AdminPlanningRepository>();
        services.AddScoped<Application.Features.AdminPreRegistration.Repositories.IAdminPreRegistrationRepository, Repositories.AdminPreRegistrationRepository>();
        services.AddScoped<Application.Features.Auth.Repositories.IAuthRepository, Repositories.AuthRepository>();
        services.AddScoped<Application.Features.StudentPreRegistration.Repositories.IStudentPreRegistrationRepository, Repositories.StudentPreRegistrationRepository>();
        services.AddScoped<Application.Features.CourseOfferings.Repositories.ICourseOfferingRepository, Repositories.CourseOfferingRepository>();
        services.AddScoped<Application.Features.TeachingAssignments.Repositories.ITeachingAssignmentRepository, Repositories.TeachingAssignmentRepository>();
        services.AddScoped<Application.Features.Enrollments.Repositories.IEnrollmentRepository, Repositories.EnrollmentRepository>();
        services.AddScoped<UniversitySystem.Application.Features.AdminReports.Repositories.IAdminReportRepository, Repositories.AdminReportRepository>();
        services.AddScoped<UniversitySystem.Application.Features.ProfessorTeachingRequests.Repositories.IProfessorTeachingRequestRepository, Repositories.ProfessorTeachingRequestRepository>();
        services.AddScoped<Application.Features.AcademicWorkflow.IAcademicWorkflowRepository, Repositories.AcademicWorkflowRepository>();
        services.AddScoped<Application.Features.MajorCurricula.IMajorCurriculumRepository, Repositories.MajorCurriculumRepository>();
        services.AddScoped<Application.Features.UiSupport.Repositories.IUiSupportRepository, Repositories.UiSupportRepository>();
        return services;
    }
}
