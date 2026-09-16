using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequest;

/// <summary>
/// درخواست خواندن اطلاعات برای «مشاهده درخواست تدریس استاد جاری»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public sealed record GetTeachingRequestQuery(long AcademicTermId) : IRequest<TeachingRequestDto?>;
