using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Queries.GetAcademicTerms;
using UniversitySystem.Application.Features.UiSupport.Queries.GetActiveCourses;
using UniversitySystem.Application.Features.UiSupport.Queries.GetActiveProfessors;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>فهرست گزینه‌های لازم فرم‌های UI؛ استاد فقط درس‌ها و آموزش فهرست استادها را دریافت می‌کند.</summary>
[ApiController]
[Route("api/v1/lookups")]
[Authorize(Roles = RoleNames.Student + "," + RoleNames.Professor + "," + RoleNames.EducationAdmin)]
public sealed class LookupController(ISender sender) : ControllerBase
{
    [HttpGet("academic-terms")]
    public async Task<ActionResult<IReadOnlyCollection<AcademicTermOptionDto>>> GetTerms(CancellationToken cancellationToken) => Ok(await sender.Send(new GetAcademicTermsQuery(), cancellationToken));
    [HttpGet("courses")]
    [Authorize(Roles = RoleNames.Professor + "," + RoleNames.EducationAdmin)]
    public async Task<ActionResult<IReadOnlyCollection<CourseOptionDto>>> GetCourses(CancellationToken cancellationToken) => Ok(await sender.Send(new GetActiveCoursesQuery(), cancellationToken));
    [HttpGet("professors")]
    [Authorize(Roles = RoleNames.EducationAdmin)]
    public async Task<ActionResult<IReadOnlyCollection<ProfessorOptionDto>>> GetProfessors(CancellationToken cancellationToken) => Ok(await sender.Send(new GetActiveProfessorsQuery(), cancellationToken));
}
