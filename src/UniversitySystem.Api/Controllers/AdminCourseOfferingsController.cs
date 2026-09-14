using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;

[ApiController]
[Route("api/v1/admin/course-offerings")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminCourseOfferingsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<CourseOfferingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<CourseOfferingDto>>> GetByTerm([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetCourseOfferingsQuery { AcademicTermId = academicTermId }, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseOfferingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseOfferingDto>> Create([FromBody] CreateCourseOfferingRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCourseOfferingCommand
        {
            AcademicTermId = request.AcademicTermId,
            CourseId = request.CourseId,
            Capacity = request.Capacity
        };
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByTerm), new { academicTermId = result.AcademicTermId }, result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(CourseOfferingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseOfferingDto>> Update([FromRoute] long id, [FromBody] UpdateCourseOfferingRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCourseOfferingCommand
        {
            Id = id,
            Capacity = request.Capacity,
            IsActive = request.IsActive
        };
        return Ok(await sender.Send(command, cancellationToken));
    }

    [HttpGet("{courseOfferingId:long}/professors")]
    [ProducesResponseType(typeof(ICollection<TeachingAssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<TeachingAssignmentDto>>> GetProfessors([FromRoute] long courseOfferingId, CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetTeachingAssignmentsQuery { CourseOfferingId = courseOfferingId }, cancellationToken));
    }

    [HttpPost("{courseOfferingId:long}/professors")]
    [ProducesResponseType(typeof(TeachingAssignmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeachingAssignmentDto>> AssignProfessor(
        [FromRoute] long courseOfferingId,
        [FromBody] AssignProfessorRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignProfessorCommand
        {
            CourseOfferingId = courseOfferingId,
            ProfessorId = request.ProfessorId
        };
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfessors), new { courseOfferingId }, result);
    }

    [HttpDelete("{courseOfferingId:long}/professors/{professorId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveProfessor(
        [FromRoute] long courseOfferingId,
        [FromRoute] long professorId,
        CancellationToken cancellationToken)
    {
        await sender.Send(new RemoveProfessorAssignmentCommand
        {
            CourseOfferingId = courseOfferingId,
            ProfessorId = professorId
        }, cancellationToken);
        return NoContent();
    }
}

public class CreateCourseOfferingRequest
{
    public long AcademicTermId { get; set; }
    public long CourseId { get; set; }
    public int Capacity { get; set; }
}

public class UpdateCourseOfferingRequest
{
    public int Capacity { get; set; }
    public bool? IsActive { get; set; }
}

public class AssignProfessorRequest
{
    public long ProfessorId { get; set; }
}
