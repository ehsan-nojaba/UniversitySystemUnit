namespace UniversitySystem.Application.Features.MajorCourses.DTOs;

/// <summary>
/// مشخصات یک درس عضو چارت رشته؛ کد، عنوان، واحد، ترم پیشنهادی و الزامی بودن را نشان می‌دهد.
/// </summary>
public sealed record MajorCourseDto(
    long CourseId,
    string Code,
    string Title,
    int Credits,
    int RecommendedTerm,
    bool IsRequired);
