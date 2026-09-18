using MediatR;
using UniversitySystem.Application.Features.MajorCourses.DTOs;

namespace UniversitySystem.Application.Features.MajorCourses.Commands.CreateCourseForMajor;

/// <summary>
/// درخواست انجام عملیات «ساخت درس جدید و افزودن به چارت رشته»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class CreateCourseForMajorCommand : IRequest<MajorCourseDto>
{
    public long MajorId { get; set; }
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int Credits { get; set; }
    public int RecommendedTerm { get; set; }
    public bool IsRequired { get; set; }
}
