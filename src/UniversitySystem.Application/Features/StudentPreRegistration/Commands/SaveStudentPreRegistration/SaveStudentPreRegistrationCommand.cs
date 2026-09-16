using MediatR;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;

/// <summary>
/// درخواست انجام عملیات «ذخیره یا ویرایش پیش‌نویس پیش‌انتخاب»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class SaveStudentPreRegistrationCommand : IRequest<StudentPreRegistrationDto>
{
    public long AcademicTermId { get; set; }
    public ICollection<SelectedCourseItemDto> Courses { get; set; } = [];
}
