using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
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
[Route("api/v1/admin/teaching-requests")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminTeachingRequestsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "مشاهده درخواست‌های تدریس استادها", Description = "عملیات مشاهده درخواست‌های تدریس استادها؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<TeachingRequestSummaryDto>))]
    public async Task<IActionResult> Get([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetTeachingRequestSummaryQuery(academicTermId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}

