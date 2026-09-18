using MediatR;
using UniversitySystem.Application.Features.MajorCourses.DTOs;
using UniversitySystem.Application.Features.MajorCourses.Services;

namespace UniversitySystem.Application.Features.MajorCourses.Commands.AddExistingCourseToMajor;

/// <summary>
/// درخواست «افزودن درس موجود به چارت رشته» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class AddExistingCourseToMajorCommandHandler(IMajorCourseService service)
    : IRequestHandler<AddExistingCourseToMajorCommand, MajorCourseDto>
{
    public Task<MajorCourseDto> Handle(AddExistingCourseToMajorCommand request, CancellationToken cancellationToken)
        => service.AddExistingCourseToMajorAsync(request.MajorId, request.CourseId, request.RecommendedTerm, request.IsRequired, cancellationToken);
}
