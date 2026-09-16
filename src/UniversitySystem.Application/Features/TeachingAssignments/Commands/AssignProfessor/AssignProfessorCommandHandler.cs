using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Services;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;

/// <summary>
/// درخواست «تخصیص استاد به ارائه» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class AssignProfessorCommandHandler(ITeachingAssignmentService service)
    : IRequestHandler<AssignProfessorCommand, TeachingAssignmentDto>
{
    public Task<TeachingAssignmentDto> Handle(AssignProfessorCommand request, CancellationToken cancellationToken)
        => service.AssignProfessorAsync(request.CourseOfferingId, request.ProfessorId, cancellationToken);
}
