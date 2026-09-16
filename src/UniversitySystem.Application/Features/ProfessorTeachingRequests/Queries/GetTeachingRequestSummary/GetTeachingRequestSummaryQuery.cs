using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequestSummary;

/// <summary>
/// درخواست خواندن اطلاعات برای «جمع‌بندی درخواست‌های ارسال‌شده استادها»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public sealed record GetTeachingRequestSummaryQuery(long AcademicTermId) : IRequest<IReadOnlyCollection<TeachingRequestSummaryDto>>;
