using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

/// <summary>
/// درخواست انجام عملیات «ارسال نهایی پیش‌انتخاب»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class SubmitStudentPreRegistrationCommand : IRequest<StudentPreRegistrationDto>
{
    public long AcademicTermId { get; set; }
}
