using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.Enrollments.Commands.CreateEnrollment;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Queries.GetStudentEnrollments;
using UniversitySystem.Application.Features.Enrollments;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «ثبت‌نام قطعی و مشاهده ثبت‌نام دانشجو»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[ApiController]
[Route("api/v1/student/enrollments")]
[Authorize(Roles = RoleNames.Student)]
public sealed class StudentEnrollmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EnrollmentDto>>> Get([FromQuery] long academicTermId, CancellationToken cancellationToken)
        => Ok(await sender.Send(new GetStudentEnrollmentsQuery(academicTermId), cancellationToken));
    [HttpPost]
    public async Task<ActionResult<EnrollmentDto>> Create(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { academicTermId = result.AcademicTermId }, result);
    }
}
