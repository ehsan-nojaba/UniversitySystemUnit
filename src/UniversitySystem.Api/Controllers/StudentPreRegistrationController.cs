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
public sealed class StudentPreRegistrationController : ControllerBase
{
    private readonly ISender _sender;

    public StudentPreRegistrationController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Retrieves courses eligible for the current student to pre-register for the given academic term.
    /// Filters courses based on active curriculum, passed course history, and satisfied prerequisites.
    /// </summary>
    /// <param name="academicTermId">The target academic term identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of eligible courses with prerequisite details.</returns>
    [HttpGet("eligible-courses")]
    [ProducesResponseType(typeof(ICollection<EligibleCourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<EligibleCourseDto>>> GetEligibleCourses([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetEligibleCoursesQuery { AcademicTermId = academicTermId }, cancellationToken));
    }

    /// <summary>
    /// Retrieves the current student's pre-registration record for an academic term.
    /// </summary>
    [HttpGet("{academicTermId:long}")]
    [ProducesResponseType(typeof(StudentPreRegistrationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentPreRegistrationDto>> GetPreRegistration([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetStudentPreRegistrationQuery { AcademicTermId = academicTermId }, cancellationToken);
        return result is null ? NotFound(new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "Not Found", Detail = "پیش‌ثبت‌نامی برای این ترم تحصیلی یافت نشد." }) : Ok(result);
    }

    /// <summary>
    /// Creates or updates the pre-registration draft for the current student in an academic term.
    /// </summary>
    [HttpPut("{academicTermId:long}")]
    [ProducesResponseType(typeof(StudentPreRegistrationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentPreRegistrationDto>> SavePreRegistration([FromRoute] long academicTermId, [FromBody] SaveStudentPreRegistrationRequest request, CancellationToken cancellationToken)
    {
        var command = new SaveStudentPreRegistrationCommand { AcademicTermId = academicTermId, Courses = request.Courses };
        return Ok(await _sender.Send(command, cancellationToken));
    }

    /// <summary>
    /// Finalizes and submits the pre-registration draft for the current student in an academic term.
    /// </summary>
    [HttpPost("{academicTermId:long}/submit")]
    [ProducesResponseType(typeof(StudentPreRegistrationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentPreRegistrationDto>> SubmitPreRegistration([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new SubmitStudentPreRegistrationCommand { AcademicTermId = academicTermId }, cancellationToken));
    }
}
