using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Domain.Enums;

/// <summary>
/// وضعیت ثبت‌نام قطعی: ثبت‌نام‌شده، گذرانده، مردود یا حذف‌شده.
/// </summary>
public enum EnrollmentStatus : byte
{
    [Display(Name = "ثبت‌نام‌شده")]
    Enrolled = 1,

    [Display(Name = "گذرانده")]
    Completed = 2,

    [Display(Name = "مردود")]
    Failed = 3,

    [Display(Name = "حذف‌شده")]
    Withdrawn = 4
}
