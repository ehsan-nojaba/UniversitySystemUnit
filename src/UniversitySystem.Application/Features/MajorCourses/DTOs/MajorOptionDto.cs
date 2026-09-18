namespace UniversitySystem.Application.Features.MajorCourses.DTOs;

/// <summary>
/// مشخصات رشته فعال برای فرم مدیریت دروس رشته؛ شناسه، کد و عنوان رشته را نگه می‌دارد.
/// </summary>
public sealed record MajorOptionDto(long Id, string Code, string Title);
