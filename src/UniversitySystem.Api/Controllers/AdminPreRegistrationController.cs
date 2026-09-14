using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;

/// <summary>
/// Admin pre-registration management and reporting endpoints.
/// Restricted strictly to EducationAdmin role.
/// </summary>
[ApiController]
[Route("api/v1/admin/pre-registration")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminPreRegistrationController : ControllerBase
{
    private readonly ISender _sender;

    public AdminPreRegistrationController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Retrieves aggregated student demand summary per course for a specified academic term.
    /// Only submitted pre-registrations are included.
    /// Results are sorted descending by demand (StudentCount).
    /// </summary>
    /// <param name="academicTermId">The target academic term identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of courses with demand metrics.</returns>
    [HttpGet("demand")]
    [ProducesResponseType(typeof(ICollection<CourseDemandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ICollection<CourseDemandDto>>> GetDemandSummary([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetCourseDemandSummaryQuery { AcademicTermId = academicTermId }, cancellationToken));
    }
}
