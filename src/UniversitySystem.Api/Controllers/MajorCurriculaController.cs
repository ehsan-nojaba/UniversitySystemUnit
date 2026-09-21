using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Contracts;
using UniversitySystem.Api.Infrastructure;
using UniversitySystem.Application.Features.MajorCurricula;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;

/// <summary>مدیریت رشته و درس‌های چارت، فقط برای آموزش؛ دانشجو رشته خود را از پروفایل دریافت می‌کند.</summary>
[Route("api/v1/admin/majors")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class MajorCurriculaController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "فهرست رشته‌ها", Description = "رشته‌ها و گروه آموزشی برای مدیریت چارت.")]
    [SwaggerResponse(200, "رشته‌ها", typeof(ICollection<MajorOptionDto>))]
    public async Task<IActionResult> GetMajors(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetMajorsQuery(), cancellationToken);
        return response.ToApiResponse();
    }
    [HttpGet("{majorId:long}/curriculum")]
    [SwaggerOperation(Summary = "چارت جاری رشته", Description = "درس‌ها، ترم پیشنهادی و الزامی بودن در آخرین چارت فعال رشته.")]
    [SwaggerResponse(200, "چارت رشته", typeof(MajorCurriculumDto))]
    public async Task<IActionResult> GetCurriculum(long majorId, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetMajorCurriculumQuery(majorId), cancellationToken);
        return response.ToApiResponse();
    }
    [HttpGet("{majorId:long}/courses/next-code")]
    [SwaggerOperation(Summary = "کد بعدی درس", Description = "کد درس جدید را با الگوی CE-Number تولید می‌کند.")]
    [SwaggerResponse(200, "کد آمادهٔ ثبت", typeof(NextCourseCodeDto))]
    public async Task<IActionResult> GetNextCourseCode(long majorId, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetNextCourseCodeQuery(majorId), cancellationToken);
        return response.ToApiResponse();
    }
    [HttpPut("{majorId:long}/curriculum")]
    [SwaggerOperation(Summary = "ذخیره درس‌های رشته", Description = "درس‌های موجود را به چارت متصل می‌کند؛ حذف فقط ارتباط با همین رشته را تغییر می‌دهد.")]
    [SwaggerResponse(200, "چارت ذخیره شد", typeof(MajorCurriculumDto))]
    public async Task<IActionResult> Save(long majorId, [FromBody] SaveMajorCurriculumRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<SaveMajorCurriculumCommand>() with { MajorId = majorId };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
    [HttpPost("{majorId:long}/courses")]
    [SwaggerOperation(Summary = "افزودن درس جدید به رشته", Description = "درس و ارتباط آن با چارت رشته در یک تراکنش ثبت می‌شوند.")]
    [SwaggerResponse(201, "درس ایجاد شد", typeof(MajorCurriculumDto))]
    public async Task<IActionResult> CreateCourse(long majorId, [FromBody] CreateMajorCourseRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<CreateMajorCourseCommand>() with { MajorId = majorId };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse(nameof(GetCurriculum), new { majorId });
    }
}

