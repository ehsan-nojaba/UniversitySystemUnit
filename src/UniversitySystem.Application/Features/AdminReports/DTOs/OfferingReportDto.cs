using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;

namespace UniversitySystem.Application.Features.AdminReports.DTOs;

/// <summary>
/// یک ردیف گزارش آموزش: درس و ارائه، ظرفیت، تعداد ثبت‌نام فعال، ظرفیت باقی‌مانده، تعداد استاد و بازه‌های کلاس.
/// </summary>
public sealed record OfferingReportDto(long CourseOfferingId, long CourseId, string Code, string Title,
    int Capacity, int EnrolledCount, int RemainingCapacity, bool IsActive, int ProfessorCount, int ScheduleCount, bool IsFinalized = false);
