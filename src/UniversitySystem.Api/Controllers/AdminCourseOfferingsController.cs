using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    [HttpGet("{courseOfferingId:long}/schedule")]
    [ProducesResponseType(typeof(ICollection<CourseOfferingScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<CourseOfferingScheduleDto>>> GetSchedule(
        [FromRoute] long courseOfferingId,
        CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetCourseOfferingScheduleQuery { CourseOfferingId = courseOfferingId }, cancellationToken));
    }

    [HttpPut("{courseOfferingId:long}/schedule")]
    [ProducesResponseType(typeof(ICollection<CourseOfferingScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<CourseOfferingScheduleDto>>> SaveSchedule(
        [FromRoute] long courseOfferingId,
        [FromBody] SaveCourseOfferingScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SaveCourseOfferingScheduleCommand
        {
            CourseOfferingId = courseOfferingId,
            Slots = request.Slots
        };
        return Ok(await sender.Send(command, cancellationToken));
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

[JsonConverter(typeof(SaveCourseOfferingScheduleRequestConverter))]
public class SaveCourseOfferingScheduleRequest
{
    public List<CourseOfferingScheduleSlotDto> Slots { get; set; } = [];
}

public class SaveCourseOfferingScheduleRequestConverter : JsonConverter<SaveCourseOfferingScheduleRequest>
{
    public override SaveCourseOfferingScheduleRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var slots = JsonSerializer.Deserialize<List<CourseOfferingScheduleSlotDto>>(ref reader, options) ?? [];
            return new SaveCourseOfferingScheduleRequest { Slots = slots };
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("slots", out var slotsProp) || root.TryGetProperty("Slots", out slotsProp))
            {
                var slots = JsonSerializer.Deserialize<List<CourseOfferingScheduleSlotDto>>(slotsProp.GetRawText(), options) ?? [];
                return new SaveCourseOfferingScheduleRequest { Slots = slots };
            }

            return new SaveCourseOfferingScheduleRequest();
        }

        throw new JsonException("Invalid JSON format for SaveCourseOfferingScheduleRequest. Expected array or object with 'slots'.");
    }

    public override void Write(Utf8JsonWriter writer, SaveCourseOfferingScheduleRequest value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("slots");
        JsonSerializer.Serialize(writer, value.Slots, options);
        writer.WriteEndObject();
    }
}
