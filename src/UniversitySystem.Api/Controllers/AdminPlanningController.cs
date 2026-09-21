using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «نمای برنامه‌ریزی آموزش»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[Route("api/v1/admin/planning")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminPlanningController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("overview")]
    [SwaggerOperation(Summary = "نمای برنامه‌ریزی آموزش", Description = "عملیات نمای برنامه‌ریزی آموزش؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(AcademicPlanningOverviewDto))]
    public async Task<IActionResult> GetOverview([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetAcademicPlanningOverviewQuery
        {
            AcademicTermId = academicTermId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}
