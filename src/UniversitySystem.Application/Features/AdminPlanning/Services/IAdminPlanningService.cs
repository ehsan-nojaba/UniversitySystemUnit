using UniversitySystem.Application.Features.AdminPlanning.DTOs;

namespace UniversitySystem.Application.Features.AdminPlanning.Services;

public interface IAdminPlanningService
{
    Task<AcademicPlanningOverviewDto> GetPlanningOverviewAsync(long academicTermId, CancellationToken cancellationToken = default);
}
