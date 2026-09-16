using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Domain.Enums;

/// <summary>
/// وضعیت سابقه درسی: در جریان، قبول، مردود یا حذف‌شده.
/// </summary>
public enum CourseEnrollmentStatus : byte
{
    [Display(Name = "در جریان")]
    InProgress = 1,

    [Display(Name = "قبول")]
    Passed = 2,

    [Display(Name = "مردود")]
    Failed = 3,

    [Display(Name = "حذف‌شده")]
    Withdrawn = 4
}
