using MediatR;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;
using UniversitySystem.Application.Features.AdminPlanning.Services;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

public sealed class GetAcademicPlanningOverviewQueryHandler(IAdminPlanningService planningService) : IRequestHandler<GetAcademicPlanningOverviewQuery, AcademicPlanningOverviewDto>
{
    public Task<AcademicPlanningOverviewDto> Handle(GetAcademicPlanningOverviewQuery request, CancellationToken cancellationToken)
        => planningService.GetPlanningOverviewAsync(request.AcademicTermId, cancellationToken);
}
