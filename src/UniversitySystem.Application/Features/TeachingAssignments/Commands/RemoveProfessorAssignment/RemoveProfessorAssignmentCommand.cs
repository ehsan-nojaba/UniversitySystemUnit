using MediatR;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;

/// <summary>
/// درخواست انجام عملیات «حذف تخصیص استاد»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class RemoveProfessorAssignmentCommand : IRequest<Unit>
{
    public long CourseOfferingId { get; set; }
    public long ProfessorId { get; set; }
}
