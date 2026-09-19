namespace UniversitySystem.Application.Features.MajorCurricula;

/// <summary>گزینه انتخاب رشته در پنل آموزش؛ گروه آموزشی برای تشخیص رشته‌های هم‌نام نمایش داده می‌شود.</summary>
public sealed record MajorOptionDto(long Id, string Code, string Title, string DepartmentTitle, bool IsActive);

/// <summary>یک درس در چارت رشته همراه ویژگی‌های مختص همان چارت.</summary>
public sealed record MajorCourseDto(long CourseId, string Code, string Title, int Credits, int RecommendedTerm, bool IsRequired, bool IsActive);

/// <summary>چارت جاری رشته؛ نبود چارت به شکل فهرست خالی برمی‌گردد و در اولین ذخیره ایجاد می‌شود.</summary>
public sealed record MajorCurriculumDto(long MajorId, string MajorTitle, long? CurriculumId, string? Title, string? Version, ICollection<MajorCourseDto> Courses);

/// <summary>ورودی ارتباط درس موجود با چارت رشته.</summary>
public sealed record CurriculumCourseInput(long CourseId, int RecommendedTerm, bool IsRequired);

/// <summary>اطلاعات ساخت درس جدید و افزودن همزمان آن به چارت رشته منتخب.</summary>
public sealed record NewMajorCourseInput(string Code, string Title, int Credits, int RecommendedTerm, bool IsRequired);
