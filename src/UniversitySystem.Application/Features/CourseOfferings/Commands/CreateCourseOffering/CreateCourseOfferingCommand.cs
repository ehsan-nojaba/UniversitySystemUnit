using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;

/// <summary>
/// درخواست انجام عملیات «ایجاد ارائه درس با ظرفیت مشخص»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class CreateCourseOfferingCommand : IRequest<CourseOfferingDto>
{
    public long AcademicTermId { get; set; }
    public long CourseId { get; set; }
    public int Capacity { get; set; }
}
