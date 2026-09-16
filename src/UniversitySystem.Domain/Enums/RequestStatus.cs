using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Domain.Enums;

/// <summary>
/// وضعیت درخواست پیش‌انتخاب یا تدریس: پیش‌نویس، ارسال‌شده و لغوشده.
/// </summary>
public enum RequestStatus : byte
{
    [Display(Name = "پیش‌نویس")]
    Draft = 1,

    [Display(Name = "ارسال‌شده")]
    Submitted = 2,

    [Display(Name = "لغوشده")]
    Cancelled = 3
}
