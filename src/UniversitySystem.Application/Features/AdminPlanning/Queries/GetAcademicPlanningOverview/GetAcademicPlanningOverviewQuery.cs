using MediatR;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

/// <summary>
/// درخواست خواندن اطلاعات برای «دریافت نمای برنامه‌ریزی آموزش»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetAcademicPlanningOverviewQuery : IRequest<AcademicPlanningOverviewDto>
{
    public long AcademicTermId { get; set; }
}
