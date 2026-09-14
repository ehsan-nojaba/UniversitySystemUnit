using FluentValidation;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;

public sealed class RemoveProfessorAssignmentCommandValidator : AbstractValidator<RemoveProfessorAssignmentCommand>
{
    public RemoveProfessorAssignmentCommandValidator()
    {
        RuleFor(x => x.CourseOfferingId)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");

        RuleFor(x => x.ProfessorId)
            .GreaterThan(0)
            .WithMessage("شناسه استاد نامعتبر است.");
    }
}
