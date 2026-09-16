using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;

/// <summary>
/// درخواست انجام عملیات «تخصیص استاد به ارائه»؛ داده ورودی عملیات را نگه می‌دارد و به Handler ارسال می‌شود.
/// </summary>
public class AssignProfessorCommand : IRequest<TeachingAssignmentDto>
{
    public long CourseOfferingId { get; set; }
    public long ProfessorId { get; set; }
}
