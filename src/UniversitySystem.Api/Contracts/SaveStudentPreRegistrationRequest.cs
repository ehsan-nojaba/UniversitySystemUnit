using UniversitySystem.Api.Serialization;
using UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;

namespace UniversitySystem.Api.Contracts;

/// <summary>
/// بدنه HTTP ذخیره پیش‌انتخاب؛ فهرست شناسه درس و اولویت دانشجو را دریافت می‌کند.
/// </summary>
public class SaveStudentPreRegistrationRequest
{
    public ICollection<SelectedCourseItemDto> Courses { get; set; } = [];
}
