using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// درخواست خواندن اطلاعات برای «دریافت درس‌های مجاز دانشجو»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetEligibleCoursesQuery : IRequest<ICollection<EligibleCourseDto>>
{
    public long AcademicTermId { get; set; }
}
