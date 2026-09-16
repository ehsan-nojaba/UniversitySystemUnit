using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;

namespace UniversitySystem.Application.Features.AdminReports.DTOs;

/// <summary>
/// گزارش یک ترم: مجموع ظرفیت ارائه‌های فعال، مجموع ثبت‌نام فعال، ردیف ارائه‌ها و درس‌های دارای تقاضا بدون ارائه فعال.
/// </summary>
public sealed record AdminReportDto(long AcademicTermId, int TotalCapacity, int TotalEnrolled,
    IReadOnlyCollection<OfferingReportDto> Offerings, IReadOnlyCollection<CoursePlanningOverviewDto> UnofferedCourses);
