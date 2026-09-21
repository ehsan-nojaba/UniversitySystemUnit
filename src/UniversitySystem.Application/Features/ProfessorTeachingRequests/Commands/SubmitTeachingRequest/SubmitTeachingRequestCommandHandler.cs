using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SubmitTeachingRequest;
/// <summary>
/// درخواست «ارسال نهایی درخواست تدریس» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class SubmitTeachingRequestCommandHandler(ProfessorTeachingRequestService service) : IRequestHandler<SubmitTeachingRequestCommand, TeachingRequestDto>
{
    public Task<TeachingRequestDto> Handle(SubmitTeachingRequestCommand r, CancellationToken ct)
    {
        return service.SubmitAsync(r.AcademicTermId, ct);
    }
}
