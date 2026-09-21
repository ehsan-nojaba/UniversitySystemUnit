using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveAvailability;
/// <summary>
/// درخواست «ثبت زمان‌های آزاد استاد» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class SaveAvailabilityCommandHandler(ProfessorTeachingRequestService service) : IRequestHandler<SaveAvailabilityCommand, TeachingRequestDto>
{
    public Task<TeachingRequestDto> Handle(SaveAvailabilityCommand r, CancellationToken ct)
    {
        return service.SaveAvailabilityAsync(r.AcademicTermId, r.Availability, ct);
    }
}
