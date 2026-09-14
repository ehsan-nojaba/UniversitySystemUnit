using MediatR;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.AdminPreRegistration.Services;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

public sealed class GetCourseDemandSummaryQueryHandler(IAdminPreRegistrationService preRegistrationService)
    : IRequestHandler<GetCourseDemandSummaryQuery, ICollection<CourseDemandDto>>
{
    public Task<ICollection<CourseDemandDto>> Handle(GetCourseDemandSummaryQuery request, CancellationToken cancellationToken)
        => preRegistrationService.GetCourseDemandSummaryAsync(request.AcademicTermId, cancellationToken);
}
