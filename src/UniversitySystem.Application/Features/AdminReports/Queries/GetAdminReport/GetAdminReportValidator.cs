using FluentValidation;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;

namespace UniversitySystem.Application.Features.AdminReports.Queries.GetAdminReport;

/// <summary>
/// اعتبارسنجی ورودی عملیات «دریافت گزارش ظرفیت و ثبت‌نام آموزش» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetAdminReportValidator : AbstractValidator<GetAdminReportQuery>
{
    public GetAdminReportValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0);
    }
}
