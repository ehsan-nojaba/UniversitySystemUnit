using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;

/// <summary>
/// درخواست انجام عملیات «ویرایش ظرفیت و وضعیت ارائه»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class UpdateCourseOfferingCommand : IRequest<CourseOfferingDto>
{
    public long Id { get; set; }
    public int Capacity { get; set; }
    public bool? IsActive { get; set; }
}
