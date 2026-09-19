using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequestSummary;
/// <summary>
/// درخواست «جمع‌بندی درخواست‌های ارسال‌شده استادها» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetTeachingRequestSummaryQueryHandler(ProfessorTeachingRequestService service) : IRequestHandler<GetTeachingRequestSummaryQuery, ICollection<TeachingRequestSummaryDto>>
{
    public Task<ICollection<TeachingRequestSummaryDto>> Handle(GetTeachingRequestSummaryQuery r, CancellationToken ct) => service.GetSummaryAsync(r.AcademicTermId, ct);
}
