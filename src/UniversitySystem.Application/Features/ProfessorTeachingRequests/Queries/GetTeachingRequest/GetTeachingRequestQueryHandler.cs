using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Queries.GetTeachingRequest;
/// <summary>
/// درخواست «مشاهده درخواست تدریس استاد جاری» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetTeachingRequestQueryHandler(ProfessorTeachingRequestService service) : IRequestHandler<GetTeachingRequestQuery, TeachingRequestDto?>
{
    public Task<TeachingRequestDto?> Handle(GetTeachingRequestQuery r, CancellationToken ct)
    {
        return service.GetAsync(r.AcademicTermId, ct);
    }
}
