using MediatR;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;
using UniversitySystem.Application.Features.AdminReports.DTOs;

namespace UniversitySystem.Application.Features.AdminReports.Queries.GetAdminReport;

/// <summary>
/// درخواست خواندن اطلاعات برای «دریافت گزارش ظرفیت و ثبت‌نام آموزش»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public sealed record GetAdminReportQuery(long AcademicTermId) : IRequest<AdminReportDto>;
