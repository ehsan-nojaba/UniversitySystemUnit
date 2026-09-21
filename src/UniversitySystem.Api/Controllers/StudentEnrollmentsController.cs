using UniversitySystem.Api.Contracts;
using Mapster;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
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
[Route("api/v1/student/enrollments")]
[Authorize(Roles = RoleNames.Student)]
public sealed class StudentEnrollmentsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "مشاهده ثبت‌نام‌های قطعی دانشجو", Description = "عملیات مشاهده ثبت‌نام‌های قطعی دانشجو؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<EnrollmentDto>))]
    public async Task<IActionResult> Get([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetStudentEnrollmentsQuery(academicTermId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPost]
    [SwaggerOperation(Summary = "ثبت‌نام قطعی در ارائه درس", Description = "عملیات ثبت‌نام قطعی در ارائه درس؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(201, "عملیات موفق", typeof(EnrollmentDto))]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<CreateEnrollmentCommand>();
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse(nameof(Get), new { academicTermId = response.AcademicTermId });
    }
}

