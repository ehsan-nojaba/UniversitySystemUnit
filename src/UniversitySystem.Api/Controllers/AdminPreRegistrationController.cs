using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «تقاضای دانشجوها برای درس‌های یک ترم»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[Route("api/v1/admin/pre-registration")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminPreRegistrationController(ISender mediator) : ApiControllerBase(mediator)
{
    /// <summary>
    /// Retrieves aggregated student demand summary per course for a specified academic term.
    /// Only submitted pre-registrations are included.
    /// Results are sorted descending by demand (StudentCount).
    /// </summary>
    /// <param name = "academicTermId">The target academic term identifier.</param>
    /// <param name = "cancellationToken">Cancellation token.</param>
    /// <returns>List of courses with demand metrics.</returns>
    [HttpGet("demand")]
    [SwaggerOperation(Summary = "مشاهده تقاضای دانشجوها", Description = "عملیات مشاهده تقاضای دانشجوها؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<CourseDemandDto>))]
    public async Task<IActionResult> GetDemandSummary([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetCourseDemandSummaryQuery
        {
            AcademicTermId = academicTermId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}
