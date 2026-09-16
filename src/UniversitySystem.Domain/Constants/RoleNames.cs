namespace UniversitySystem.Domain.Constants;

/// <summary>
/// نام ثابت نقش‌های دسترسی؛ برای یکسان بودن نام نقش در مجوز API و توکن استفاده می‌شود.
/// </summary>
public static class RoleNames
{
    public const string Student = "Student";
    public const string Professor = "Professor";
    public const string EducationAdmin = "EducationAdmin";

    /// <summary>
    /// Collection of all predefined system role names.
    /// </summary>
    public static readonly IReadOnlyList<string> All = new[]
    {
        Student,
        Professor,
        EducationAdmin
    };
}
