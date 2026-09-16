using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetCurrentUser;
/// <summary>درخواست خواندن GetCurrentUser برای راه‌اندازی UI.</summary>
public sealed record GetCurrentUserQuery : IRequest<CurrentUserDto>;
