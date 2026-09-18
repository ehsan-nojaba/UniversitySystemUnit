using MediatR;
using UniversitySystem.Application.Features.MajorCourses.DTOs;

namespace UniversitySystem.Application.Features.MajorCourses.Commands.AddExistingCourseToMajor;

/// <summary>
/// درخواست انجام عملیات «افزودن درس موجود به چارت رشته»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class AddExistingCourseToMajorCommand : IRequest<MajorCourseDto>
{
    public long MajorId { get; set; }
    public long CourseId { get; set; }
    public int RecommendedTerm { get; set; }
    public bool IsRequired { get; set; }
}
