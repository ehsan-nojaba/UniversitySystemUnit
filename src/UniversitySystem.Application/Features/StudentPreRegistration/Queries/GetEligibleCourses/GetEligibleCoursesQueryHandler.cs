using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Services;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// درخواست «دریافت درس‌های مجاز دانشجو» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetEligibleCoursesQueryHandler(IStudentPreRegistrationService service)
    : IRequestHandler<GetEligibleCoursesQuery, ICollection<EligibleCourseDto>>
{
    public Task<ICollection<EligibleCourseDto>> Handle(GetEligibleCoursesQuery request, CancellationToken cancellationToken)
    {
        return service.GetEligibleCoursesAsync(request.AcademicTermId, cancellationToken);
    }
}
