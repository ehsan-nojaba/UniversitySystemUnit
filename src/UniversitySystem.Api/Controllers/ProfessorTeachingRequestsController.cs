using UniversitySystem.Application.Features.AcademicWorkflow;
using Mapster;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using UniversitySystem.Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveAvailability;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SubmitTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.ReopenTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequest;
using UniversitySystem.Application.Features.ProfessorTeachingRequests;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «ثبت، مشاهده و ارسال درخواست تدریس و زمان آزاد استاد»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[Route("api/v1/professor/teaching-request")]
[Authorize(Roles = RoleNames.Professor)]
public sealed class ProfessorTeachingRequestsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("final-schedule")]
    [SwaggerOperation(Summary = "برنامه نهایی کلاس‌های استاد", Description = "کلاس‌هایی که آموزش برای استاد جاری نهایی و منتشر کرده است.")]
    [SwaggerResponse(200, "برنامه نهایی", typeof(ICollection<FinalTeachingOffering>))]
    public async Task<IActionResult> GetFinalSchedule([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new MyFinalScheduleQuery(academicTermId), cancellationToken);
        return response.ToApiResponse();
    }
    [HttpGet("eligible-courses")]
    [SwaggerOperation(Summary = "درس‌های مجاز استاد و تقاضای دانشجو", Description = "فقط درس‌های مرتبط با استاد جاری همراه تعداد درخواست‌های ارسال‌شده در ترم انتخابی.")]
    [SwaggerResponse(200, "درس‌ها", typeof(ICollection<ProfessorCourseOption>))]
    public async Task<IActionResult> GetEligibleCourses([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new MyTeachingCoursesQuery(academicTermId), cancellationToken);
        return response.ToApiResponse();
    }

    [HttpGet("{academicTermId:long}")]
    [SwaggerOperation(Summary = "مشاهده درخواست تدریس و زمان‌های آزاد استاد", Description = "عملیات مشاهده درخواست تدریس و زمان‌های آزاد استاد؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(TeachingRequestDto))]
    public async Task<IActionResult> Get([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetTeachingRequestQuery(academicTermId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPut("{academicTermId:long}")]
    [SwaggerOperation(Summary = "ذخیره درخواست تدریس", Description = "عملیات ذخیره درخواست تدریس؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(TeachingRequestDto))]
    public async Task<IActionResult> Save([FromRoute] long academicTermId, [FromBody] SaveProfessorTeachingRequestRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<SaveTeachingRequestCommand>() with { AcademicTermId = academicTermId };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPut("{academicTermId:long}/availability")]
    [SwaggerOperation(Summary = "ذخیره زمان‌های آزاد استاد", Description = "عملیات ذخیره زمان‌های آزاد استاد؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(TeachingRequestDto))]
    public async Task<IActionResult> SaveAvailability([FromRoute] long academicTermId, [FromBody] SaveProfessorAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<SaveAvailabilityCommand>() with { AcademicTermId = academicTermId };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPost("{academicTermId:long}/submit")]
    [SwaggerOperation(Summary = "ارسال نهایی درخواست تدریس", Description = "عملیات ارسال نهایی درخواست تدریس؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(TeachingRequestDto))]
    public async Task<IActionResult> Submit([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new SubmitTeachingRequestCommand(academicTermId);
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPost("{academicTermId:long}/reopen")]
    [SwaggerOperation(Summary = "بازکردن درخواست برای ویرایش", Description = "استاد تا قبل از تخصیص درس توسط آموزش می‌تواند درخواست ارسال‌شده را دوباره برای افزودن یا ویرایش درس و زمان باز کند.")]
    [SwaggerResponse(200, "درخواست برای ویرایش باز شد", typeof(TeachingRequestDto))]
    public async Task<IActionResult> Reopen([FromRoute] long academicTermId, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new ReopenTeachingRequestCommand(academicTermId), cancellationToken);
        return response.ToApiResponse();
    }
}

