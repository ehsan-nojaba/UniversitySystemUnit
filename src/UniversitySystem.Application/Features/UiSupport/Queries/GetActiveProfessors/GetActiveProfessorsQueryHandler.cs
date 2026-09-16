using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Services;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetActiveProfessors;
/// <summary>تحویل درخواست خواندن اطلاعات UI به سرویس، بدون وابستگی به EF.</summary>
public sealed class GetActiveProfessorsQueryHandler(UiSupportService service) : IRequestHandler<GetActiveProfessorsQuery, IReadOnlyCollection<ProfessorOptionDto>>
{
    public Task<IReadOnlyCollection<ProfessorOptionDto>> Handle(GetActiveProfessorsQuery request, CancellationToken cancellationToken) => service.GetProfessorsAsync(cancellationToken);
}
