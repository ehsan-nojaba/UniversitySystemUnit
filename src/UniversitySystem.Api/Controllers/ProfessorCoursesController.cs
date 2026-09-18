using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using UniversitySystem.Application.Features.AcademicWorkflow;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;

/// <summary>آموزش ارتباط پایه استاد و درس‌های مجاز او را مدیریت می‌کند.</summary>
[ApiController]
[Route("api/v1/admin/professors")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class ProfessorCoursesController(ISender _mediator) : ControllerBase
{
    [HttpGet("{professorId:long}/courses")]
    [SwaggerOperation(Summary = "درس‌های مجاز استاد", Description = "شناسه درس‌هایی که این استاد اجازه پیشنهاد تدریس آن‌ها را دارد.")]
    [SwaggerResponse(200, "درس‌های استاد", typeof(IReadOnlyCollection<long>))]
    public async Task<IActionResult> Get(long professorId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ProfessorCoursesQuery(professorId), cancellationToken);
        return response.ToApiResponse();
    }
    [HttpPut("{professorId:long}/courses")]
    [SwaggerOperation(Summary = "تعیین درس‌های مجاز استاد", Description = "ارتباط پایه استاد و درس را ثبت می‌کند؛ استاد کلاس و ساعت نهایی در برنامه‌ریزی ترم تعیین می‌شوند.")]
    [SwaggerResponse(200, "درس‌ها ذخیره شدند", typeof(IReadOnlyCollection<long>))]
    public async Task<IActionResult> Save(long professorId, [FromBody] SaveProfessorCoursesRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<SaveProfessorCoursesCommand>() with { ProfessorId = professorId };
        var response = await _mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}

/// <summary>درس‌های مجاز منتخب آموزش برای یک استاد.</summary>
public sealed record SaveProfessorCoursesRequest(IReadOnlyCollection<long> CourseIds);
