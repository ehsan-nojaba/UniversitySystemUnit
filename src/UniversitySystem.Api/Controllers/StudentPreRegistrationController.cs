using Mapster;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Api.Contracts;
using UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «درس‌های مجاز، ذخیره و ارسال پیش‌انتخاب دانشجو»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[ApiController]
[Route("api/v1/student/pre-registration")]
[Authorize(Roles = RoleNames.Student)]
public sealed class StudentPreRegistrationController(ISender mediator) : ApiControllerBase(mediator)
{
    /// <summary>
    /// Retrieves courses eligible for the current student to pre-register for the given academic term.
    /// Filters courses based on active curriculum, passed course history, and satisfied prerequisites.
    /// </summary>
    /// <param name = "academicTermId">The target academic term identifier.</param>
    /// <param name = "cancellationToken">Cancellation token.</param>
    /// <returns>List of eligible courses with prerequisite details.</returns>
    [HttpGet("eligible-courses")]
    [SwaggerOperation(Summary = "مشاهده درس‌های مجاز", Description = "عملیات مشاهده درس‌های مجاز؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<EligibleCourseDto>))]
    public async Task<IActionResult> GetEligibleCourses([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetEligibleCoursesQuery
        {
            AcademicTermId = academicTermId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    /// <summary>
    /// Retrieves the current student's pre-registration record for an academic term.
    /// </summary>
    [HttpGet("{academicTermId:long}")]
    [SwaggerOperation(Summary = "مشاهده پیش‌انتخاب", Description = "عملیات مشاهده پیش‌انتخاب؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(StudentPreRegistrationDto))]
    public async Task<IActionResult> GetPreRegistration([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetStudentPreRegistrationQuery { AcademicTermId = academicTermId };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    /// <summary>
    /// Creates or updates the pre-registration draft for the current student in an academic term.
    /// </summary>
    [HttpPut("{academicTermId:long}")]
    [SwaggerOperation(Summary = "ذخیره پیش‌انتخاب", Description = "عملیات ذخیره پیش‌انتخاب؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(StudentPreRegistrationDto))]
    public async Task<IActionResult> SavePreRegistration([FromRoute] long academicTermId, [FromBody] SaveStudentPreRegistrationRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<SaveStudentPreRegistrationCommand>();
        param.AcademicTermId = academicTermId;
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    /// <summary>
    /// Finalizes and submits the pre-registration draft for the current student in an academic term.
    /// </summary>
    [HttpPost("{academicTermId:long}/submit")]
    [SwaggerOperation(Summary = "ارسال نهایی پیش‌انتخاب", Description = "عملیات ارسال نهایی پیش‌انتخاب؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(StudentPreRegistrationDto))]
    public async Task<IActionResult> SubmitPreRegistration([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new SubmitStudentPreRegistrationCommand
        {
            AcademicTermId = academicTermId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}
