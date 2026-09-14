using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;

[ApiController]
[Route("api/v1/admin/planning")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminPlanningController : ControllerBase
{
    private readonly ISender _sender;

    public AdminPlanningController(ISender sender) => _sender = sender;

    [HttpGet("overview")]
    [ProducesResponseType(typeof(AcademicPlanningOverviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcademicPlanningOverviewDto>> GetOverview([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetAcademicPlanningOverviewQuery { AcademicTermId = academicTermId }, cancellationToken));
    }
}
