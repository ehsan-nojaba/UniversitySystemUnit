using MediatR;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

/// <summary>
/// درخواست خواندن اطلاعات برای «دریافت تقاضای درس از درخواست‌های ارسال‌شده»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetCourseDemandSummaryQuery : IRequest<ICollection<CourseDemandDto>>
{
    public long AcademicTermId { get; set; }
}
