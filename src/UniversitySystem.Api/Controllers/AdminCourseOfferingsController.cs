using UniversitySystem.Application.Features.AcademicWorkflow;
using Mapster;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Api.Contracts;
using UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «ارائه درس، تخصیص استاد و برنامه زمانی کلاس»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
[Route("api/v1/admin/course-offerings")]
[Authorize(Roles = RoleNames.EducationAdmin)]
public sealed class AdminCourseOfferingsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("{id:long}/finalize")]
    [SwaggerOperation(Summary = "نهایی‌کردن برنامه کلاس", Description = "پس از کنترل استاد، پیشنهاد زمان، ظرفیت و تداخل، ارائه را برای دانشجو منتشر می‌کند.")]
    [SwaggerResponse(200, "برنامه نهایی شد", typeof(bool))]
    public async Task<IActionResult> FinalizeOffering(long id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new FinalizeOfferingCommand(id, true), cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPost("{id:long}/reopen")]
    [SwaggerOperation(Summary = "بازگشت ارائه به برنامه‌ریزی", Description = "تا پیش از ثبت‌نام دانشجو، انتشار را متوقف و ویرایش برنامه را باز می‌کند.")]
    [SwaggerResponse(200, "برنامه قابل ویرایش است", typeof(bool))]
    public async Task<IActionResult> ReopenOffering(long id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new FinalizeOfferingCommand(id, false), cancellationToken);
        return response.ToApiResponse();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "مشاهده ارائه‌های ترم", Description = "عملیات مشاهده ارائه‌های ترم؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<CourseOfferingDto>))]
    public async Task<IActionResult> GetByTerm([FromQuery] long academicTermId, CancellationToken cancellationToken)
    {
        var param = new GetCourseOfferingsQuery
        {
            AcademicTermId = academicTermId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPost]
    [SwaggerOperation(Summary = "ثبت اطلاعات جدید", Description = "عملیات ثبت اطلاعات جدید؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(201, "عملیات موفق", typeof(CourseOfferingDto))]
    public async Task<IActionResult> Create([FromBody] CreateCourseOfferingRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<CreateCourseOfferingCommand>();
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse(nameof(GetByTerm), new { academicTermId = response.AcademicTermId });
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "ویرایش ارائه", Description = "عملیات ویرایش ارائه؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(CourseOfferingDto))]
    public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateCourseOfferingRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<UpdateCourseOfferingCommand>();
        param.Id = id;
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpGet("{courseOfferingId:long}/professors")]
    [SwaggerOperation(Summary = "مشاهده استادهای تخصیص‌یافته", Description = "عملیات مشاهده استادهای تخصیص ‌یافته؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<TeachingAssignmentDto>))]
    public async Task<IActionResult> GetProfessors([FromRoute] long courseOfferingId, CancellationToken cancellationToken)
    {
        var param = new GetTeachingAssignmentsQuery
        {
            CourseOfferingId = courseOfferingId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPost("{courseOfferingId:long}/professors")]
    [SwaggerOperation(Summary = "تخصیص استاد", Description = "عملیات تخصیص استاد؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(201, "عملیات موفق", typeof(TeachingAssignmentDto))]
    public async Task<IActionResult> AssignProfessor([FromRoute] long courseOfferingId, [FromBody] AssignProfessorRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<AssignProfessorCommand>();
        param.CourseOfferingId = courseOfferingId;
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse(nameof(GetProfessors), new { courseOfferingId });
    }

    [HttpDelete("{courseOfferingId:long}/professors/{professorId:long}")]
    [SwaggerOperation(Summary = "حذف تخصیص استاد", Description = "عملیات حذف تخصیص استاد؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(204, "عملیات موفق")]
    public async Task<IActionResult> RemoveProfessor([FromRoute] long courseOfferingId, [FromRoute] long professorId, CancellationToken cancellationToken)
    {
        var param = new RemoveProfessorAssignmentCommand { CourseOfferingId = courseOfferingId, ProfessorId = professorId };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse(204);
    }

    [HttpGet("{courseOfferingId:long}/schedule")]
    [SwaggerOperation(Summary = "مشاهده برنامه کلاس", Description = "عملیات مشاهده برنامه کلاس؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<CourseOfferingScheduleDto>))]
    public async Task<IActionResult> GetSchedule([FromRoute] long courseOfferingId, CancellationToken cancellationToken)
    {
        var param = new GetCourseOfferingScheduleQuery
        {
            CourseOfferingId = courseOfferingId
        };
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }

    [HttpPut("{courseOfferingId:long}/schedule")]
    [SwaggerOperation(Summary = "ذخیره برنامه کلاس", Description = "عملیات ذخیره برنامه کلاس؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(ICollection<CourseOfferingScheduleDto>))]
    public async Task<IActionResult> SaveSchedule([FromRoute] long courseOfferingId, [FromBody] SaveCourseOfferingScheduleRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<SaveCourseOfferingScheduleCommand>();
        param.CourseOfferingId = courseOfferingId;
        var response = await Mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}

