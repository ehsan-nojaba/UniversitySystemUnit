using MediatR;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;
using UniversitySystem.Application.Features.AdminReports.DTOs;
using UniversitySystem.Application.Features.AdminReports.Services;

namespace UniversitySystem.Application.Features.AdminReports.Queries.GetAdminReport;

/// <summary>
/// درخواست «دریافت گزارش ظرفیت و ثبت‌نام آموزش» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetAdminReportHandler(AdminReportService service) : IRequestHandler<GetAdminReportQuery, AdminReportDto>
{
    public Task<AdminReportDto> Handle(GetAdminReportQuery request, CancellationToken cancellationToken)
    {
        return service.GetAsync(request.AcademicTermId, cancellationToken);
    }
}
