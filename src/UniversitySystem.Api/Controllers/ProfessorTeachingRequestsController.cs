using UniversitySystem.Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveAvailability;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SubmitTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «ثبت، مشاهده و ارسال درخواست تدریس و زمان آزاد استاد»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[ApiController]
[Route("api/v1/professor/teaching-request")]
[Authorize(Roles = RoleNames.Professor)]
public sealed class ProfessorTeachingRequestsController(ISender sender) : ControllerBase
{
    [HttpGet("{academicTermId:long}")]
    public async Task<ActionResult<TeachingRequestDto>> Get(long academicTermId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTeachingRequestQuery(academicTermId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
    [HttpPut("{academicTermId:long}")]
    public async Task<ActionResult<TeachingRequestDto>> Save(long academicTermId, SaveProfessorTeachingRequestRequest request, CancellationToken cancellationToken)
        => Ok(await sender.Send(new SaveTeachingRequestCommand(academicTermId, request.Courses), cancellationToken));
    [HttpPut("{academicTermId:long}/availability")]
    public async Task<ActionResult<TeachingRequestDto>> SaveAvailability(long academicTermId, SaveProfessorAvailabilityRequest request, CancellationToken cancellationToken)
        => Ok(await sender.Send(new SaveAvailabilityCommand(academicTermId, request.Availability), cancellationToken));
    [HttpPost("{academicTermId:long}/submit")]
    public async Task<ActionResult<TeachingRequestDto>> Submit(long academicTermId, CancellationToken cancellationToken)
        => Ok(await sender.Send(new SubmitTeachingRequestCommand(academicTermId), cancellationToken));
}
