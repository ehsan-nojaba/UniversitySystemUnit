using MediatR;
using UniversitySystem.Application.Features.MajorCourses.DTOs;
using UniversitySystem.Application.Features.MajorCourses.Services;

namespace UniversitySystem.Application.Features.MajorCourses.Commands.CreateCourseForMajor;

/// <summary>
/// درخواست «ساخت درس جدید و افزودن به چارت رشته» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class CreateCourseForMajorCommandHandler(IMajorCourseService service)
    : IRequestHandler<CreateCourseForMajorCommand, MajorCourseDto>
{
    public Task<MajorCourseDto> Handle(CreateCourseForMajorCommand request, CancellationToken cancellationToken)
        => service.CreateCourseForMajorAsync(request.MajorId, request.Code, request.Title, request.Credits, request.RecommendedTerm, request.IsRequired, cancellationToken);
}
