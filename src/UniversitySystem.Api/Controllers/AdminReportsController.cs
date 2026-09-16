using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.AdminReports.DTOs;
using UniversitySystem.Application.Features.AdminReports.Queries.GetAdminReport;
using UniversitySystem.Application.Features.AdminReports;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «گزارش ظرفیت، ثبت‌نام و درس‌های بدون ارائه»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[ApiController]
[Route("api/v1/admin/reports")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminReportsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AdminReportDto>> Get([FromQuery] long academicTermId, CancellationToken cancellationToken)
        => Ok(await sender.Send(new GetAdminReportQuery(academicTermId), cancellationToken));
}
