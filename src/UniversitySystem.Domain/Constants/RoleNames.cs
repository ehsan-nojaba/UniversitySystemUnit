namespace UniversitySystem.Domain.Constants;

/// <summary>
/// Defines the central constant role names used for Role-Based Authorization across the system.
/// Corresponds to records in the Role entity table.
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
