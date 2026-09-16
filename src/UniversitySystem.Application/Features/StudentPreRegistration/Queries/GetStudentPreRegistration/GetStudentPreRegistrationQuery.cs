using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;

/// <summary>
/// درخواست خواندن اطلاعات برای «مشاهده پیش‌انتخاب دانشجو»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetStudentPreRegistrationQuery : IRequest<StudentPreRegistrationDto?>
{
    public long AcademicTermId { get; set; }
}
