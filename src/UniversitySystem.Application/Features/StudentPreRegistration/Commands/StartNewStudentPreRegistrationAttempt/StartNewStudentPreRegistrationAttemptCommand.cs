using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.StartNewStudentPreRegistrationAttempt;

/// <summary>
/// شروع نوبت دوم پیش‌انتخاب برای همان ترم؛ فقط پس از ارسال نهایی نوبت قبلی قابل اجراست.
/// </summary>
public class StartNewStudentPreRegistrationAttemptCommand : IRequest<StudentPreRegistrationDto>
{
    public long AcademicTermId { get; set; }
}
