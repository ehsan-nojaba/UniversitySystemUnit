using UniversitySystem.Application.Features.MajorCurricula;

namespace UniversitySystem.Api.Contracts;

/// <summary>فهرست کامل درس‌ها و ویژگی‌های آن‌ها در چارت رشته منتخب.</summary>
public sealed record SaveMajorCurriculumRequest(ICollection<CurriculumCourseInput> Courses);
/// <summary>ساخت درس جدید و اتصال همزمان به رشته بدون تغییر چارت سایر رشته‌ها؛ کد درس خودکار ساخته می‌شود.</summary>
public sealed record CreateMajorCourseRequest(string Title, int Credits, int RecommendedTerm, bool IsRequired);
