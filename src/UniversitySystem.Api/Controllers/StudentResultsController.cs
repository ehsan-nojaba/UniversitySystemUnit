using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
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
public sealed class StudentResultsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("{academicTermId:long}/result")]
    [SwaggerOperation(Summary = "مشاهده نتیجه پیش‌انتخاب و ارائه‌های قابل ثبت‌نام", Description = "عملیات مشاهده نتیجه پیش‌انتخاب و ارائه‌های قابل ثبت‌نام؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(StudentResultDto))]
    public async Task<IActionResult> Get([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetStudentResultQuery(academicTermId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}

