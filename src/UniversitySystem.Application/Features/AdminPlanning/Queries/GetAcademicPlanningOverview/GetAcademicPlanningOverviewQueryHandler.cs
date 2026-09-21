using MediatR;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

/// <summary>
/// درخواست «دریافت نمای برنامه‌ریزی آموزش» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetAcademicPlanningOverviewQueryHandler(IAdminPlanningService planningService) : IRequestHandler<GetAcademicPlanningOverviewQuery, AcademicPlanningOverviewDto>
{
    public Task<AcademicPlanningOverviewDto> Handle(GetAcademicPlanningOverviewQuery request, CancellationToken cancellationToken)
    {
        return planningService.GetPlanningOverviewAsync(request.AcademicTermId, cancellationToken);
    }
}
