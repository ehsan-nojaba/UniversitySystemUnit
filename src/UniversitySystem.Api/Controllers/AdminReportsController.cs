using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
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
public sealed class AdminReportsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "مشاهده اطلاعات", Description = "عملیات مشاهده اطلاعات؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(AdminReportDto))]
    public async Task<IActionResult> Get([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetAdminReportQuery(academicTermId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}

