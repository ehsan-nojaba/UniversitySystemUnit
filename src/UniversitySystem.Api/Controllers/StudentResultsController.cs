using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.StudentResults.DTOs;
using UniversitySystem.Application.Features.StudentResults.Queries.GetStudentResult;
using UniversitySystem.Application.Features.StudentResults;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «نتیجه پیش‌انتخاب دانشجو»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[ApiController]
[Route("api/v1/student/pre-registration")]
[Authorize(Roles = RoleNames.Student)]
public sealed class StudentResultsController(ISender sender) : ControllerBase
{
    [HttpGet("{academicTermId:long}/result")]
    public async Task<ActionResult<StudentResultDto>> Get(long academicTermId, CancellationToken cancellationToken)
        => Ok(await sender.Send(new GetStudentResultQuery(academicTermId), cancellationToken));
}
