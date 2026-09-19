using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetActiveProfessors;
/// <summary>درخواست خواندن GetActiveProfessors برای راه‌اندازی UI.</summary>
public sealed record GetActiveProfessorsQuery : IRequest<ICollection<ProfessorOptionDto>>;
