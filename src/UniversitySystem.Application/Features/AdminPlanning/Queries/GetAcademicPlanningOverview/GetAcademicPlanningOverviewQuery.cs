using MediatR;
using UniversitySystem.Application.Features.AdminPlanning.DTOs;

namespace UniversitySystem.Application.Features.AdminPlanning.Queries.GetAcademicPlanningOverview;

public class GetAcademicPlanningOverviewQuery : IRequest<AcademicPlanningOverviewDto>
{
    public long AcademicTermId { get; set; }
}
