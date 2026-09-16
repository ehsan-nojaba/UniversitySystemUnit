using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Services;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetCurrentUser;
/// <summary>تحویل درخواست خواندن اطلاعات UI به سرویس، بدون وابستگی به EF.</summary>
public sealed class GetCurrentUserQueryHandler(UiSupportService service) : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    public Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken) => service.GetCurrentUserAsync(cancellationToken);
}
