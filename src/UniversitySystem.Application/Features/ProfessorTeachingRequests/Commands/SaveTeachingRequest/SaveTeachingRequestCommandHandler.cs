using MediatR;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Services;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveTeachingRequest;
/// <summary>
/// درخواست «ذخیره یا ویرایش درس‌های درخواست تدریس» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class SaveTeachingRequestCommandHandler(ProfessorTeachingRequestService service) : IRequestHandler<SaveTeachingRequestCommand, TeachingRequestDto>
{
    public Task<TeachingRequestDto> Handle(SaveTeachingRequestCommand r, CancellationToken ct) => service.SaveAsync(r.AcademicTermId, r.Courses, ct);
}
