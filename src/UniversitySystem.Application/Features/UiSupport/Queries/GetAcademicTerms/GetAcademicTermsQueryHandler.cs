using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Services;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetAcademicTerms;
/// <summary>تحویل درخواست خواندن اطلاعات UI به سرویس، بدون وابستگی به EF.</summary>
public sealed class GetAcademicTermsQueryHandler(UiSupportService service) : IRequestHandler<GetAcademicTermsQuery, ICollection<AcademicTermOptionDto>>
{
    public Task<ICollection<AcademicTermOptionDto>> Handle(GetAcademicTermsQuery request, CancellationToken cancellationToken) => service.GetTermsAsync(cancellationToken);
}
