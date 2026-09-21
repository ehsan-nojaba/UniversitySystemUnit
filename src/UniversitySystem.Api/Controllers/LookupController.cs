using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
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
[Route("api/v1/lookups")]
[Authorize(Roles = RoleNames.Student + "," + RoleNames.Professor + "," + RoleNames.EducationAdmin)]
public sealed class LookupController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("academic-terms")]
    [SwaggerOperation(Summary = "فهرست ترم‌ها", Description = "عملیات فهرست ترم‌ها؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<AcademicTermOptionDto>))]
    public async Task<IActionResult> GetTerms(CancellationToken cancellationToken)
    {
        var param = new GetAcademicTermsQuery();
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpGet("courses")]
    [Authorize(Roles = RoleNames.Professor + "," + RoleNames.EducationAdmin)]
    [SwaggerOperation(Summary = "فهرست درس‌های فعال", Description = "عملیات فهرست درس‌های فعال؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<CourseOptionDto>))]
    public async Task<IActionResult> GetCourses([FromQuery] long? majorId, CancellationToken cancellationToken)
    {
        var param = new GetActiveCoursesQuery(majorId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpGet("courses/catalog")]
    [Authorize(Roles = RoleNames.EducationAdmin)]
    [SwaggerOperation(Summary = "فهرست درس‌های قابل افزودن", Description = "فهرست درس‌های فعال گروه آموزشی رشته برای افزودن به چارت؛ ارتباط درس با رشته پس از ذخیره چارت ثبت می‌شود.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<CourseOptionDto>))]
    public async Task<IActionResult> GetCourseCatalog([FromQuery] long? majorId, CancellationToken cancellationToken)
    {
        var param = new GetActiveCoursesQuery(majorId, IncludeAll: true);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpGet("professors")]
    [Authorize(Roles = RoleNames.EducationAdmin)]
    [SwaggerOperation(Summary = "فهرست استادهای فعال", Description = "عملیات فهرست استادهای فعال؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<ProfessorOptionDto>))]
    public async Task<IActionResult> GetProfessors(CancellationToken cancellationToken)
    {
        var param = new GetActiveProfessorsQuery();
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}

