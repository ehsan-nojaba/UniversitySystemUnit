using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Repositories;

/// <summary>
/// داده موردنیاز بررسی مجاز بودن درس: درس‌های چارت با پیش‌نیازها و مجموعه شناسه درس‌های پاس‌شده.
/// </summary>
public sealed record EligibilityData(ICollection<EligibleCourseDto> Courses, IReadOnlySet<long> PassedCourseIds);
