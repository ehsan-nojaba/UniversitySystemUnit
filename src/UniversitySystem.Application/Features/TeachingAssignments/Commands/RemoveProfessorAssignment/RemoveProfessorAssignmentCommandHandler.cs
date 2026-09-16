using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.Services;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;

/// <summary>
/// درخواست «حذف تخصیص استاد» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class RemoveProfessorAssignmentCommandHandler(ITeachingAssignmentService service)
    : IRequestHandler<RemoveProfessorAssignmentCommand, Unit>
{
    public async Task<Unit> Handle(RemoveProfessorAssignmentCommand request, CancellationToken cancellationToken)
    {
        await service.RemoveAssignmentAsync(request.CourseOfferingId, request.ProfessorId, cancellationToken);
        return Unit.Value;
    }
}
