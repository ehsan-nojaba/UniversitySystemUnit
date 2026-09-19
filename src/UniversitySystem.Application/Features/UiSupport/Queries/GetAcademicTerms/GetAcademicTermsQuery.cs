using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetAcademicTerms;
/// <summary>درخواست خواندن GetAcademicTerms برای راه‌اندازی UI.</summary>
public sealed record GetAcademicTermsQuery : IRequest<ICollection<AcademicTermOptionDto>>;
