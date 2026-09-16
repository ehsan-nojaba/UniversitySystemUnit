using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequestSummary;
using UniversitySystem.Application.Features.ProfessorTeachingRequests;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «مشاهده درخواست‌های تدریس ارسال‌شده استادها»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[ApiController]
[Route("api/v1/admin/teaching-requests")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminTeachingRequestsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TeachingRequestSummaryDto>>> Get([FromQuery] long academicTermId, CancellationToken cancellationToken)
        => Ok(await sender.Send(new GetTeachingRequestSummaryQuery(academicTermId), cancellationToken));
}
