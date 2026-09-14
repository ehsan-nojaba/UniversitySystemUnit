using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Services;

public interface IAdminPreRegistrationService
{
    Task<ICollection<CourseDemandDto>> GetCourseDemandSummaryAsync(long academicTermId, CancellationToken cancellationToken = default);
}
